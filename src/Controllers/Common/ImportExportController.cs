using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ExportController : ControllerBase {
    private readonly DBContext ctx;

    public ExportController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Route("Export")]
    public IActionResult Export([FromForm] string username, [FromForm] string password) {
        // Authenticate user by Username
        User? user = ctx.Users.FirstOrDefault(e => e.Username == username);
        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Failed) {
            return Unauthorized("Invalid username or password.");
        }

        // Serialize to JSON
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        string jsonData = JsonSerializer.Serialize(user, options);

        return Ok(jsonData);
    }

    [HttpPost]
    [Route("Import")]
    public IActionResult Import([FromForm] string username, [FromForm] string password, [FromForm] string vikingName, [FromForm] IFormFile dataFile, [FromForm] string? newVikingName) {
        User? user = ctx.Users.FirstOrDefault(e => e.Username == username);
        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Failed) {
            return Unauthorized("Invalid username or password.");
        }

        User user_data;
        using (var reader = new StreamReader(dataFile.OpenReadStream())) {
            user_data = System.Text.Json.JsonSerializer.Deserialize<User>(reader.ReadToEnd());
        }

        foreach (var v in user_data.Vikings) {
            if (v.Name == vikingName) {
                if (String.IsNullOrEmpty(newVikingName))
                    newVikingName = vikingName;

                if (ctx.Vikings.Count(e => e.Name == newVikingName) > 0) {
                    return Conflict("Viking name already in use");
                }

                if (newVikingName != vikingName) {
                    AvatarData avatarData = XmlUtil.DeserializeXml<AvatarData>(v.AvatarSerialized);
                    avatarData.DisplayName = newVikingName;
                    v.AvatarSerialized = XmlUtil.SerializeXml(avatarData);
                }

                Viking viking = new Viking {
                    Uid = Guid.NewGuid(),
                    Name = newVikingName,
                    User = user,
                    AvatarSerialized = v.AvatarSerialized,
                    CreationDate = DateTime.UtcNow,
                    BirthDate = v.BirthDate,
                    Gender = v.Gender,
                    GameVersion = v.GameVersion
                };
                user.Vikings.Add(viking);

                Dictionary<int, Guid> dragonIds = new();
                foreach (var x in v.Dragons) {
                    x.Viking = viking;
                    x.EntityId = Guid.NewGuid();
                    dragonIds.Add(x.Id, x.EntityId);
                    x.Id = 0;
                    ctx.Dragons.Add(x);
                }
                Dictionary<int, int> itemIds = new();
                foreach (var x in v.InventoryItems) {
                    itemIds.Add(x.Id, x.ItemId);
                    x.Id = 0;
                    x.Viking = viking;
                    ctx.InventoryItems.Add(x);
                }

                ctx.SaveChanges(); // need for get new ids of dragons and items

                HashSet<int> usedItemIds = new();
                foreach (var x in v.Rooms) {
                    x.Viking = viking;
                    if (int.TryParse(x.RoomId, out int roomID)) {
                        // numeric room name is inventory item id
                        // remap old value to new value based on item id value
                        roomID = viking.InventoryItems.FirstOrDefault(e => e.ItemId == itemIds[roomID] && !usedItemIds.Contains(e.Id)).Id;
                        usedItemIds.Add(roomID);
                        x.RoomId = roomID.ToString();
                    }
                    ctx.Rooms.Add(x);
                }
                foreach (var x in v.PairData) {
                    x.Viking = viking;
                    if (x.PairId == 2014) { // stables data
                        foreach (var p in x.Pairs.Where(e => e.Key.StartsWith("Stable"))) {
                            StableData stableData = XmlUtil.DeserializeXml<StableData>(p.Value);
                            stableData.InventoryID = viking.InventoryItems.FirstOrDefault(e => e.ItemId == stableData.ItemID).Id;
                            usedItemIds.Add(stableData.InventoryID);
                            foreach (var n in stableData.NestList) {
                                if (n.PetID != 0)
                                    n.PetID = viking.Dragons.FirstOrDefault(d => d.EntityId == dragonIds[n.PetID]).Id;
                            }
                            p.Value = XmlUtil.SerializeXml(stableData);
                        }
                    }
                    ctx.PairData.Add(x);
                }

                foreach (var x in v.Images) {
                    x.Viking = viking;
                    ctx.Images.Add(x);
                }
                foreach (var x in v.MissionStates) {
                    x.Viking = viking;
                    ctx.MissionStates.Add(x);
                }
                foreach (var x in v.TaskStatuses) {
                    x.Viking = viking;
                    ctx.TaskStatuses.Add(x);
                }
                foreach (var x in v.AchievementTaskStates) {
                    x.Viking = viking;
                    ctx.AchievementTaskState.Add(x);
                }
                foreach (var x in v.AchievementPoints) {
                    x.Viking = viking;
                    ctx.AchievementPoints.Add(x);
                }
                foreach (var x in v.ProfileAnswers) {
                    x.Viking = viking;
                    ctx.ProfileAnswers.Add(x);
                }
                foreach (var x in v.GameData) {
                    x.Viking = viking;
                    ctx.GameData.Add(x);
                }
                foreach (var x in v.SavedData) {
                    x.Viking = viking;
                    ctx.SavedData.Add(x);
                }
                foreach (var x in v.Parties) {
                    x.Viking = viking;
                    ctx.Parties.Add(x);
                }
                foreach (var x in v.UserMissions) {
                    x.Viking = viking;
                    ctx.UserMissions.Add(x);
                }
                foreach (var x in v.UserBadgesCompleted) {
                    x.Viking = viking;
                    ctx.UserBadgesCompleted.Add(x);
                }
                if (v.Ratings.Count > 0) {
                    viking.Ratings = new List<Rating>();
                    foreach (var x in v.Ratings) {
                        // TODO (non-SoD) add rating via SetRating(viking, x.Rank.CategoryID, x.Rank.RatedEntityID, x.Rank.RatedUserID, x.Value);
                    }
                }
                if (v.Neighborhood != null) {
                    v.Neighborhood.Viking = viking;
                    ctx.Neighborhoods.Add(v.Neighborhood);
                }

                if (v.SelectedDragon != null)
                    viking.SelectedDragon = viking.Dragons.FirstOrDefault(d => d.EntityId == dragonIds[v.SelectedDragon.Id]);

                ctx.SaveChanges();
                return Ok("OK");
            }
        }
        return Ok("Viking Not Found");
    }

    [HttpGet]
    [Route("Export")]
    public IActionResult Export() {
        return Content(XmlUtil.ReadResourceXmlString("html.export"), "application/xhtml+xml");
    }

    [HttpGet]
    [Route("Import")]
    public IActionResult Import() {
        return Content(XmlUtil.ReadResourceXmlString("html.import"), "application/xhtml+xml");
    }
}
