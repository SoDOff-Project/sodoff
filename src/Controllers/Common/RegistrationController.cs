using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class RegistrationController : Controller {

    private readonly DBContext ctx;
    private ItemService itemService;
    private MissionService missionService;
    private RoomService roomService;
    private KeyValueService keyValueService;

    public RegistrationController(DBContext ctx, ItemService itemService, MissionService missionService, RoomService roomService, KeyValueService keyValueService) {
        this.ctx = ctx;
        this.itemService = itemService;
        this.missionService = missionService;
        this.roomService = roomService;
        this.keyValueService = keyValueService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/RegistrationWebService.asmx/DeleteProfile")]
    public IActionResult DeleteProfile([FromForm] Guid apiToken, [FromForm] Guid userID) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null) {
            return Ok(DeleteProfileStatus.OWNER_ID_NOT_FOUND);
        }

        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == userID);
        if (viking is null) {
            return Ok(DeleteProfileStatus.PROFILE_NOT_FOUND);
        }

        if (user != viking.User) {
            return Ok(DeleteProfileStatus.PROFILE_NOT_OWNED_BY_THIS_OWNER);
        }

        ctx.Vikings.Remove(viking);
        ctx.SaveChanges();

        return Ok(DeleteProfileStatus.SUCCESS);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/RegistrationWebService.asmx/RegisterParent")]
    [DecryptRequest("parentRegistrationData")]
    [EncryptResponse]
    public IActionResult RegisterParent() {
        ParentRegistrationData data = XmlUtil.DeserializeXml<ParentRegistrationData>(Request.Form["parentRegistrationData"]);
        User u = new User {
            Id = Guid.NewGuid(),
            Username = data.ChildList[0].ChildName,
            Password = new PasswordHasher<object>().HashPassword(null, data.Password),
            Email = data.Email
        };

        // Check if user exists
        if (ctx.Users.Count(e => e.Username== u.Username) > 0) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateUserName });
        }  

        ctx.Users.Add(u);
        ctx.SaveChanges();

        ParentLoginInfo pli = new ParentLoginInfo {
            UserName = u.Username,
            ApiToken = Guid.NewGuid().ToString(),
            UserID = u.Id.ToString(),
            Status = MembershipUserStatus.Success,
            UnAuthorized = false
        };

        var response = new RegistrationResult {
            ParentLoginInfo = pli,
            UserID = u.Id.ToString(),
            Status = MembershipUserStatus.Success,
            ApiToken = Guid.NewGuid().ToString()
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V3/RegistrationWebService.asmx/RegisterChild")] // used by Magic & Mythies
    [Route("V4/RegistrationWebService.asmx/RegisterChild")]
    [DecryptRequest("childRegistrationData")]
    [EncryptResponse]
    public IActionResult RegisterChild([FromForm] Guid parentApiToken, [FromForm] string apiKey) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
        if (user is null) {
            return Ok(new RegistrationResult{
                Status = MembershipUserStatus.InvalidApiToken
            });
        }

        // Check if name populated
        ChildRegistrationData data = XmlUtil.DeserializeXml<ChildRegistrationData>(Request.Form["childRegistrationData"]);
        if (String.IsNullOrWhiteSpace(data.ChildName)) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.ValidationError });
        }

        // Check if viking exists
        if (ctx.Vikings.Count(e => e.Name == data.ChildName) > 0) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateUserName });
        }

        List<InventoryItem> items = new() {
            new InventoryItem { ItemId = 8977, Quantity = 1 } // DragonStableINTDO - Dragons Dragon Stable
        };

        Viking v = new Viking {
            Uid = Guid.NewGuid(),
            Name = data.ChildName,
            User = user,
            InventoryItems = items,
            AchievementPoints = new List<AchievementPoints>(),
            Rooms = new List<Room>()
        };

        uint gameVersion = ClientVersion.GetVersion(apiKey);
        missionService.SetUpMissions(v, gameVersion);

        ctx.Vikings.Add(v);
        ctx.SaveChanges();

        if (gameVersion < 0xa2a09a0a) {
            keyValueService.SetPairData(null, v, null, 2017, new Schema.PairData {
                Pairs = new Schema.Pair[]{
                    new Schema.Pair {
                        // avoid show change viking name dialog
                        PairKey = "AvatarNameCustomizationDone",
                        PairValue = "1"
                    },
                }
            });
        }

        roomService.CreateRoom(v, "MyRoomINT");

        return Ok(new RegistrationResult {
            UserID = v.Uid.ToString(),
            Status = MembershipUserStatus.Success
        });
    }
}
