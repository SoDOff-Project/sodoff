using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class AchievementController : Controller {

    public readonly DBContext ctx;
    private AchievementStoreSingleton achievementStore;
    private AchievementService achievementService;
    
    public AchievementController(DBContext ctx, AchievementStoreSingleton achievementStore, AchievementService achievementService) {
        this.ctx = ctx;
        this.achievementStore = achievementStore;
        this.achievementService = achievementService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetPetAchievementsByUserID")]
    public IActionResult GetPetAchievementsByUserID([FromForm] string userId) {
        // NOTE: this is public info (for mmo) - no session check
        List<UserAchievementInfo> dragonsAchievement = new List<UserAchievementInfo>();
        foreach (Dragon dragon in ctx.Dragons.Where(d => d.Viking.Uid == Guid.Parse(userId))) {
            dragonsAchievement.Add(
                achievementService.CreateUserAchievementInfo(dragon.EntityId, dragon.PetXP, AchievementPointTypes.DragonXP)
            );
        }

        ArrayOfUserAchievementInfo arrAchievements = new ArrayOfUserAchievementInfo {
            UserAchievementInfo = dragonsAchievement.ToArray()
        };

        return Ok(arrAchievements);
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAllRanks")]
    public IActionResult GetAllRanks([FromForm] string apiKey) {
        uint gameVersion = ClientVersion.GetVersion(apiKey);
        if (gameVersion <= ClientVersion.Max_OldJS && (gameVersion & ClientVersion.WoJS) != 0)
            return Ok(XmlUtil.ReadResourceXmlString("ranks.allranks_wojs"));
        if (gameVersion == ClientVersion.MB)
            return Ok(XmlUtil.ReadResourceXmlString("ranks.allranks_mb"));
        return Ok(XmlUtil.ReadResourceXmlString("ranks.allranks_sod"));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAchievementTaskInfo")]
    public IActionResult GetAchievementTaskInfo([FromForm] string achievementTaskIDList, [FromForm] string apiKey) {
        int[] achievementTaskIDs = XmlUtil.DeserializeXml<int[]>(achievementTaskIDList);
        List<AchievementTaskInfo> achievementTaskInfos = new();
        foreach (int taskId in achievementTaskIDs) {
            var achievementInfo = achievementStore.GetAllAchievementTaskInfo(taskId, ClientVersion.GetVersion(apiKey));
            if (achievementInfo != null)
                achievementTaskInfos.AddRange(achievementInfo);
        }
        return Ok(new ArrayOfAchievementTaskInfo {
            AchievementTaskInfo = achievementTaskInfos.ToArray()
        });
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAllRewardTypeMultiplier")]
    public IActionResult GetAllRewardTypeMultiplier() {
        // TODO
        return Ok(XmlUtil.ReadResourceXmlString("rewardmultiplier"));
    }

    [HttpPost]
    [Route("AchievementWebService.asmx/SetDragonXP")]  // used by dragonrescue-import
    [VikingSession]
    public IActionResult SetDragonXP(Viking viking, [FromForm] string dragonId, [FromForm] int value) {
        Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.EntityId == Guid.Parse(dragonId));
        if (dragon is null) {
            return Conflict("Dragon not found");
        }

        dragon.PetXP = value;
        ctx.SaveChanges();

        return Ok("OK");
    }

    [HttpPost]
    [Route("AchievementWebService.asmx/SetPlayerXP")]  // used by dragonrescue-import
    [VikingSession]
    public IActionResult SetDragonXP(Viking viking, [FromForm] int type, [FromForm] int value) {
        if (!Enum.IsDefined(typeof(AchievementPointTypes), type)) {
            return Conflict("Invalid XP type");
        }

        AchievementPointTypes xpType = (AchievementPointTypes)type;
        achievementService.SetAchievementPoints(viking, xpType, value);
        ctx.SaveChanges();
        return Ok("OK");
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetUserAchievementInfo")] // used by World Of Jumpstart
    public IActionResult GetUserAchievementInfo([FromForm] Guid apiToken) {
        Viking? viking = ctx.Sessions.FirstOrDefault(x => x.ApiToken == apiToken).Viking;
        
        if (viking != null) {
            return Ok(
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerXP)
            );
        }
        return null;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAchievementsByUserID")]
    public IActionResult GetAchievementsByUserID([FromForm] string userId) {
        // NOTE: this is public info (for mmo) - no session check
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == Guid.Parse(userId));
        if (viking != null) {
            return Ok(new ArrayOfUserAchievementInfo {
                UserAchievementInfo = new UserAchievementInfo[]{
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerXP),
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFarmingXP),
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFishingXP),
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.UDTPoints),
                }
            });
        }

        Dragon? dragon = ctx.Dragons.FirstOrDefault(e => e.EntityId == Guid.Parse(userId));
        if (dragon != null) {
            return Ok(new ArrayOfUserAchievementInfo {
                UserAchievementInfo = new UserAchievementInfo[]{
                    achievementService.CreateUserAchievementInfo(dragon.EntityId, dragon.PetXP, AchievementPointTypes.DragonXP),
                }
            });
        }

        return null;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetUserAchievements")] // used by Magic & Mythies
    [VikingSession]
    public IActionResult GetUserAchievements(Viking viking) {
        ArrayOfUserAchievementInfo arrAchievements = new ArrayOfUserAchievementInfo {
            UserAchievementInfo = new UserAchievementInfo[]{
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFarmingXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFishingXP),
            }
        };

        return Ok(arrAchievements);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/SetAchievementAndGetReward")]
    [Route("AchievementWebService.asmx/SetUserAchievementAndGetReward")]
    [VikingSession(UseLock=true)]
    public IActionResult SetAchievementAndGetReward(Viking viking, [FromForm] int achievementID) {
        var rewards = achievementService.ApplyAchievementRewardsByID(viking, achievementID);
        ctx.SaveChanges();

        return Ok(rewards);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/AchievementWebService.asmx/SetUserAchievementTask")]
    [DecryptRequest("achievementTaskSetRequest")]
    [VikingSession(UseLock=true)]
    public IActionResult SetUserAchievementTask(Viking viking, [FromForm] string apiKey) {
        AchievementTaskSetRequest request = XmlUtil.DeserializeXml<AchievementTaskSetRequest>(Request.Form["achievementTaskSetRequest"]);

        var response = new List<AchievementTaskSetResponse>();
        foreach (var task in request.AchievementTaskSet) {
            response.Add(
                achievementService.ApplyAchievementRewardsByTask(viking, task.TaskID, ClientVersion.GetVersion(apiKey))
            );
        }
        ctx.SaveChanges();

        return Ok(new ArrayOfAchievementTaskSetResponse { AchievementTaskSetResponse = response.ToArray() });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/SetUserAchievementTask")] // used by World Of Jumpstart
    [VikingSession(UseLock=true)]
    public IActionResult SetUserAchievementTaskV1(Viking viking, [FromForm] int taskID, [FromForm] string apiKey) {
        var response = achievementService.ApplyAchievementRewardsByTask(viking, taskID, ClientVersion.GetVersion(apiKey));
        ctx.SaveChanges();

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetUserAchievementTask")]
    // [VikingSession]
    public IActionResult GetUserAchievementTask([FromForm] string userId, [FromForm] string apiKey) {
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == Guid.Parse(userId));
        if (viking is null)
            return Ok(new ArrayOfUserAchievementTask());
        return Ok(new ArrayOfUserAchievementTask {
            UserAchievementTask = achievementService.GetUserAchievementTask(viking, ClientVersion.GetVersion(apiKey))
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/AchievementWebService.asmx/GetUserAchievementTaskRedeemableRewards")]
    // [VikingSession]
    public IActionResult GetUserAchievementTaskRedeemableRewards() {
        return Ok(); // TODO: this should be <UATRRS>
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/ApplyPayout")]
    [VikingSession]
    public IActionResult ApplyPayout(Viking viking, string ModuleName, int points) {
        // TODO: use args (ModuleName and points) to calculate reward
        var rewards = new AchievementReward[]{
            achievementService.AddAchievementPoints(viking, AchievementPointTypes.PlayerXP, 10),
            achievementService.AddAchievementPoints(viking, AchievementPointTypes.GameCurrency, 5),
            achievementService.AddAchievementPoints(viking, AchievementPointTypes.DragonXP, 6),
            achievementService.AddAchievementPoints(viking, AchievementPointTypes.UDTPoints, 6),
        };
        ctx.SaveChanges();
        
        return Ok(rewards);
    }
    
    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/SetAchievementByEntityIDs")]
    [VikingSession(UseLock=true)]
    public IActionResult SetAchievementByEntityIDs(Viking viking, [FromForm] int achievementID, [FromForm] string petIDs) {
        Guid[] petGuids = XmlUtil.DeserializeXml<Guid[]>(petIDs);

        var rewards = achievementService.ApplyAchievementRewardsByID(viking, achievementID, petGuids);
        ctx.SaveChanges();
        
        return Ok(rewards);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/AchievementWebService.asmx/GetTopAchievementPointUsers")]
    [VikingSession]
    public IActionResult GetTopAchievementPointUsers(string request) {
        UserAchievementInfoRequest infoRequest = XmlUtil.DeserializeXml<UserAchievementInfoRequest>(request);
        return Ok(achievementService.GetTopAchievementUsers(infoRequest));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetTopAchievementPointBuddiesByType")] // Used by Math Blaster
    [VikingSession]
    public IActionResult GetTopAchievementPointBuddiesByType([FromForm] string userId, [FromForm] int pointTypeID) {
        UserAchievementInfoRequest infoRequest = new UserAchievementInfoRequest {
            UserID = new Guid(userId),
            PointTypeID = pointTypeID
        };
        return Ok(achievementService.GetTopAchievementBuddies(infoRequest));
    }
}
