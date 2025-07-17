using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using sodoff.Model;

namespace sodoff.Controllers.Common;
public class ExportController : ControllerBase {
    private readonly DBContext ctx;

    public ExportController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Route("ImportExport.asmx/Export")]
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
    [Route("ImportExport.asmx/Import")]
    public IActionResult Import([FromForm] string username, [FromForm] string password, [FromForm] string vikingName, [FromForm] IFormFile dataFile) {
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
                Viking viking = new Viking {
                    Uid = v.Uid, // TODO check for unique or just generate new?
                    Name = v.Name, // TODO check for unique
                    User = user,
                    AvatarSerialized = v.AvatarSerialized,
                    CreationDate = v.CreationDate, // TODO or use now?
                    BirthDate = v.BirthDate,
                    Gender = v.Gender,
                    GameVersion = v.GameVersion
                };
                user.Vikings.Add(viking);

                foreach (var x in v.Dragons) {
                    x.Viking = viking;
                    // TODO check EntityId for unique or just generate new?
                    x.Id = 0; // FIXME map old→new value for dragon id  to update (stables) xml's
                    ctx.Dragons.Add(x);
                }
                foreach (var x in v.Images) {
                    x.Viking = viking;
                    ctx.Images.Add(x);
                }
                foreach (var x in v.InventoryItems) {
                    x.Id = 0; // FIXME map old→new value for item id  to update xml's and rooms
                    x.Viking = viking;
                    ctx.InventoryItems.Add(x);
                }
                foreach (var x in v.Rooms) {
                    x.Viking = viking;
                    ctx.Rooms.Add(x);  // FIXME need update room name (if numeric)
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
                foreach (var x in v.PairData) {
                    x.Viking = viking;
                    ctx.PairData.Add(x);  // FIXME need update PetID in stable XML
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
                // TODO set viking.SelectedDragon

                ctx.SaveChanges();
                return Ok("OK");
            }
        }
        return Ok("Viking Not Found");
    }
}
