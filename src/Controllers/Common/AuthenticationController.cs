using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

[ApiController]
public class AuthenticationController : Controller {

    private readonly DBContext ctx;

    public AuthenticationController(DBContext ctx) {
        this.ctx = ctx;
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
    public IActionResult LoginParent() {
        ParentLoginData data = XmlUtil.DeserializeXml<ParentLoginData>(Request.Form["parentLoginData"]);

        // Authenticate the user
        User? user = ctx.Users.FirstOrDefault(e => e.Username == data.UserName);
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

        var response = new ParentLoginInfo {
            UserName = user.Username,
            //Email = user.Email, /* disabled to avoid put email in client debug logs */
            ApiToken = session.ApiToken.ToString(),
            UserID = user.Id.ToString(),
            Status = MembershipUserStatus.Success,
            SendActivationReminder = false,
            UnAuthorized = false
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
                MultiplayerEnabled = ClientVersion.IsMultiplayerSupported(apiKey),
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
                MultiplayerEnabled = ClientVersion.IsMultiplayerSupported(apiKey),
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
    [Route("AuthenticationWebService.asmx/IsValidApiToken_V2")]
    public IActionResult IsValidApiToken([FromForm] Guid apiToken) {
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
    public IActionResult LoginChild([FromForm] Guid parentApiToken) {
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
}
