using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class GroupController : Controller {
    public static readonly Schema.Group EMD_Dragons = new Schema.Group {
        GroupID = "8e68214a-c801-4759-8461-d01f28484134",
        Name = "Dragons",
        Color = "234,57,23",
        Logo = "RS_DATA/Content/PlayerData/EMD/IcoEMDTeamDragons.png"
    };
    public static readonly Schema.Group EMD_Scorpions = new Schema.Group {
        GroupID = "db0aa225-2f0e-424c-83a7-73783fe63fef",
        Name = "Scorpions",
        Color = "120,183,53",
        Logo = "RS_DATA/Content/PlayerData/EMD/IcoEMDTeamScorpions.png"
    };


    private readonly DBContext ctx;

    public GroupController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("GroupWebService.asmx/JoinGroup")]
    [VikingSession]
    public IActionResult JoinGroup(Viking viking, [FromForm] string apiKey, [FromForm] string groupID) {
        AddEMDGroups();
        uint version = ClientVersion.GetVersion(apiKey);

        // Only implemented for EMD so far.
        if (version == ClientVersion.EMD) {
            if (viking.Groups.Any(g => {
                // Check for loyalty.
                string id = g.GroupID.ToString();
                return id == EMD_Dragons.GroupID || id == EMD_Scorpions.GroupID;
            })) {
                return Ok(new JoinGroupResult { GroupStatus = GroupMembershipStatus.ALREADY_MEMBER });
            }
            groupID = groupID.ToUpper();
            Model.Group? group = ctx.Groups.FirstOrDefault(g => g.GroupID.ToString() == groupID);
            if (group != null) {
                group.Vikings.Add(viking);
                ctx.SaveChanges();
                return Ok(new JoinGroupResult { GroupStatus = GroupMembershipStatus.APPROVED });
            }
        }
        return Ok(new JoinGroupResult { GroupStatus = GroupMembershipStatus.REJECTED });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("GroupWebService.asmx/GetGroupsByGroupType")]
    [VikingSession]
    public Schema.Group[] GetGroupsByGroupType([FromForm] string apiKey, [FromForm] string groupType) {
        AddEMDGroups();
        List<Schema.Group> groups = new List<Schema.Group>();
        foreach (Model.Group group in ctx.Groups) {
            if (group.ApiKey == apiKey && group.Type.ToString() == groupType) groups.Add(new Schema.Group {
                GroupID = group.GroupID.ToString(),
                Name = group.Name,
                Color = group.Color,
                Logo = group.Logo,
                Type = group.Type
            });
        }
        return groups.ToArray();
    }

    private void AddEMDGroups() {
        bool changed = false;
        Guid DragonString = new Guid(EMD_Dragons.GroupID);
        Guid ScorpionString = new Guid(EMD_Scorpions.GroupID);
        if (!ctx.Groups.Any(g => g.GroupID == DragonString)) {
            ctx.Groups.Add(new Model.Group {
                GroupID = DragonString,
                Name = EMD_Dragons.Name,
                Color = EMD_Dragons.Color,
                Logo = EMD_Dragons.Logo,
                Type = GroupType.System,
                ApiKey = "dd602cf1-cc98-4738-9a0a-56dde3026947"
            });
            changed = true;
        }
        if (!ctx.Groups.Any(g => g.GroupID == ScorpionString)) {
            ctx.Groups.Add(new Model.Group {
                GroupID = ScorpionString,
                Name = EMD_Scorpions.Name,
                Color = EMD_Scorpions.Color,
                Logo = EMD_Scorpions.Logo,
                Type = GroupType.System,
                ApiKey = "dd602cf1-cc98-4738-9a0a-56dde3026947"
            });
            changed = true;
        }
        if (changed) ctx.SaveChanges();
    }
}
