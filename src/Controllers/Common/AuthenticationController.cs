using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;
using sodoff.Configuration;

namespace sodoff.Controllers.Common;

[ApiController]
public class AuthenticationController : Controller {

    private readonly DBContext ctx;
    private readonly IOptions<ApiServerConfig> config;

    public AuthenticationController(DBContext ctx, IOptions<ApiServerConfig> config) {
        this.ctx = ctx;
        this.config = config;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/GetRules")]
    [EncryptResponse]
    public IActionResult GetRules() {
        GetProductRulesResponse response = new GetProductRulesResponse {
            GlobalSecretKey = "11A0CC5A-C4DF-4A0E-931C-09A44C9966AE"
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/LoginParent")]
    [DecryptRequest("parentLoginData")]
    [EncryptResponse]
    public IActionResult LoginParent([FromForm] string apiKey) {
        ParentLoginData data = XmlUtil.DeserializeXml<ParentLoginData>(Request.Form["parentLoginData"]);

        // Authenticate the user
        User? user = null;
        uint gameVersion = ClientVersion.GetVersion(apiKey);
        if (gameVersion <= ClientVersion.Max_OldJS) {
            user = ctx.Users.FirstOrDefault(e => e.Email == data.UserName);
        } else {
            user = ctx.Users.FirstOrDefault(e => e.Username == data.UserName);
        }

        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, data.Password) != PasswordVerificationResult.Success) {
            return Ok(new ParentLoginInfo { Status = MembershipUserStatus.InvalidPassword });
        }

        // Create session
        Session session = new Session {
            User = user,
            ApiToken = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        ctx.Sessions.Add(session);
        ctx.SaveChanges();

        var childList = new List<sodoff.Schema.UserLoginInfo>();
        foreach (var viking in user.Vikings) {
            childList.Add(new sodoff.Schema.UserLoginInfo{UserName = viking.Name, UserID = viking.Uid.ToString()});
        }

        var response = new ParentLoginInfo {
            UserName = user.Username,
            //Email = user.Email, /* disabled to avoid put email in client debug logs */
            ApiToken = session.ApiToken.ToString(),
            UserID = user.Id.ToString(),
            Status = MembershipUserStatus.Success,
            SendActivationReminder = false,
            UnAuthorized = false,
            ChildList = childList.ToArray()
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/AuthenticateUser")]
    [DecryptRequest("username")]
    [DecryptRequest("password")]
    public bool AuthenticateUser() {
        String username = Request.Form["username"];
        String password = Request.Form["password"];

        // Authenticate the user
        User? user = ctx.Users.FirstOrDefault(e => e.Username == username);
        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, password) != PasswordVerificationResult.Success) {
            return false;
        }

        return true;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/GetUserInfoByApiToken")]
    public IActionResult GetUserInfoByApiToken([FromForm] Guid apiToken, [FromForm] string apiKey) {
        // First check if this is a user session
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is not null) {
            return Ok(new UserInfo {
                UserID = user.Id.ToString(),
                Username = user.Username,
                MembershipID = "ef84db9-59c6-4950-b8ea-bbc1521f899b", // placeholder
                FacebookUserID = 0,
                MultiplayerEnabled = ClientVersion.GetVersion(apiKey) >= config.Value.MMOSupportMinVersion,
                IsApproved = true,
                Age = 24,
                OpenChatEnabled = true
            });
        }

        // Then check if this is a viking session
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is not null)
        {
            return Ok(new UserInfo {
                UserID = viking.Uid.ToString(),
                Username = viking.Name,
                FacebookUserID = 0,
                MultiplayerEnabled = ClientVersion.GetVersion(apiKey) >= config.Value.MMOSupportMinVersion,
                IsApproved = true,
                Age = 24,
                OpenChatEnabled = true
            });
        }

        // Otherwise, this is a bad session, return empty UserInfo
        return Ok(new UserInfo {});
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/IsValidApiToken")] // used by World Of Jumpstart (FutureLand)
    public IActionResult IsValidApiToken_V1([FromForm] Guid? apiToken) {
        if (apiToken is null)
            return Ok(false);
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (user is null && viking is null)
            return Ok(false);
        return Ok(true);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/IsValidApiToken_V2")]
    public IActionResult IsValidApiToken([FromForm] Guid? apiToken) {
        if (apiToken is null)
            return Ok(ApiTokenStatus.TokenNotFound);
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (user is null && viking is null)
            return Ok(ApiTokenStatus.TokenNotFound);
        return Ok(ApiTokenStatus.TokenValid);
    }

    // This is more of a "create session for viking", rather than "login child"
    [Route("AuthenticationWebService.asmx/LoginChild")]
    [DecryptRequest("childUserID")]
    [EncryptResponse]
    public IActionResult LoginChild([FromForm] Guid parentApiToken, [FromForm] string apiKey) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
        if (user is null) {
            return Unauthorized();
        }

        // Find the viking
        string? childUserID = Request.Form["childUserID"];
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == Guid.Parse(childUserID));
        if (viking is null) {
            return Unauthorized();
        }

        uint gameVersion = ClientVersion.GetVersion(apiKey);
        if (viking.GameVersion is null)
            viking.GameVersion = gameVersion;
        if (
            (viking.GameVersion != gameVersion) &&
            !(viking.GameVersion >= ClientVersion.Min_SoD && gameVersion >= ClientVersion.Min_SoD) &&
            !(viking.GameVersion >= ClientVersion.WoJS && gameVersion >= ClientVersion.WoJS && viking.GameVersion < ClientVersion.WoJS_NewAvatar && gameVersion < ClientVersion.WoJS_NewAvatar)
        )
            return Unauthorized();
            // do not let players log into users from other games, exceptions:
            //   1) different version of SoD
            //   2) WoJS with old avatar and lands

        // Check if user is viking parent
        if (user != viking.User) {
            return Unauthorized();
        }

        // Create session
        Session session = new Session {
            Viking = viking,
            ApiToken = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        ctx.Sessions.Add(session);
        ctx.SaveChanges();

        // Return back the api token
        return Ok(session.ApiToken.ToString());
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/DeleteAccountNotification")]
    public IActionResult DeleteAccountNotification([FromForm] Guid apiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null)
            return Ok(MembershipUserStatus.ValidationError);

        ctx.Users.Remove(user);
        ctx.SaveChanges();

        return Ok(MembershipUserStatus.Success);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("Authentication/MMOAuthentication")]
    public IActionResult MMOAuthentication([FromForm] Guid token) {
        AuthenticationInfo info = new();
        info.Authenticated = false;
        var session = ctx.Sessions.FirstOrDefault(x => x.ApiToken == token);
        if (session != null) {
            info.Authenticated = true;
            info.DisplayName = session.Viking.Name;
            Role? role = session.Viking.MMORoles.FirstOrDefault()?.Role;
            if (role != null)
                info.Role = (Role)role;
        }
        return Ok(info);
    }
}
