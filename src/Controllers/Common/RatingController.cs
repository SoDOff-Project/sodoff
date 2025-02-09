using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class RatingController : Controller
{
    private readonly DBContext ctx;

    public RatingController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MissionWebService.asmx/GetPayout")] // used by World Of Jumpstart
        public IActionResult GetPayout([FromForm] int points, [FromForm] string ModuleName) {
        switch (ModuleName)
        {
            case string x when x.StartsWith("MSBejeweled"):
                return Ok(points / (350 / 3));

            case string x when x.StartsWith("MSDodgeNDash"):
                return Ok((int)Math.Floor(Math.Pow(1.145, (double)points / 10)));

            case string x when x.StartsWith("MSBuggyRacer"):
                return Ok(points / 4.7);

            case string x when x.StartsWith("MSSoundBop"):
                return Ok(points * 10);

            case string x when x.StartsWith("MSGhostTownGrab"):
                return Ok(points / 20);

            case string x when x.StartsWith("BubbleTrouble"):
                return Ok(points / 10);

            case string x when x.StartsWith("HopsJetPack"):
                return Ok(points / 25);

            case string x when x.StartsWith("HallowsBubbleMaze"):
                return Ok(points / 10);

            case string x when x.StartsWith("MSRoboAGoGo"):
                return Ok(points / 20);

            case string x when x.StartsWith("Volleyball"):
                return Ok(points / 1.8);

            case string x when x.StartsWith("Football"):
                return Ok(points / 3.2);

            case string x when x.StartsWith("Basketball"):
                return Ok(points / 1.8);

            case string x when x.StartsWith("FruitSaladChop"):
                return Ok(points / 60);

            case string x when x.StartsWith("JSGardenDefense"):
                return Ok(points / (350/3));

            case string x when x.StartsWith("JSSushiChop"):
                return Ok(points / 120);

            case string x when x.StartsWith("JSDanceOff"):
                return Ok(points / (350/3));

            case string x when x.StartsWith("MSPnCow"):
                return Ok(60);

            case string x when x.StartsWith("MSPnBunny"):
                return Ok(60);

            case string x when x.StartsWith("MSPnMonkey"):
                return Ok(60);

            case string x when x.StartsWith("MSCnKite"):
                return Ok(60);

            case string x when x.StartsWith("MSCnLadyBug"):
                return Ok(60);

            case string x when x.StartsWith("MSCnDucky"):
                return Ok(60);

            case string x when x.StartsWith("Calendar"):
                        return Ok(points / 40);

            case string x when x.StartsWith("PearlPush"):
                        return Ok(points / 20);

            case string x when x.StartsWith("FLLetItRide"):
                        return Ok(points <= 28 ? 2 : 5);

            case string x when x.StartsWith("FLLetItRideMastered"):
                        return Ok(points <= 15 ? 4 :
                                points <= 25 ? 6 :
                                points <= 35 ? 9 :
                                points <= 45 ? 12 : 15);

            case string x when x.StartsWith("FashionShowWin"):
                        return Ok(points >= 1 ? 20 : 0);

            case string x when x.StartsWith("FashionShowLose"):
                        return Ok(points >= 0 ? 10 : 0);

            case string x when x.StartsWith("FashionShowTie"):
                        return Ok(points >= 2 ? 15 : 0);
        }

        return Ok(points / (350 / 3));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ScoreWebService.asmx/SetScore")] // used by World Of Jumpstart
    public IActionResult SetScore()
    {
        // TODO - placeholder
        return Ok(true);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/Ratingwebservice.asmx/GetAverageRatingForRoom")]
    //[VikingSession(UseLock=false)]
    public IActionResult GetAverageRatingForRoom(/*Viking viking,*/ [FromForm] string request)
    {
        // TODO: This is a placeholder
        return Ok(5);
    }

    // This method is the only thing that adds ratings.
    private RatingInfo SetRating(Viking viking, int category, int? eID, string? uID, int value) {
        RatingRank? rank;
        Rating? rating = viking.Ratings.FirstOrDefault(
            r => category == r.Rank.CategoryID && r.Rank.RatedEntityID == eID && r.Rank.RatedUserID == uID
        );
        if (rating == null) {
            rank = ctx.RatingRanks.FirstOrDefault(rr => rr.CategoryID == category && rr.RatedEntityID == eID && rr.RatedUserID == uID);
            if (rank == null) {
                rank = new RatingRank {
                    CategoryID = category,
                    RatedEntityID = eID,
                    RatedUserID = uID,
                    Rank = 0,
                    Ratings = new List<Rating>()
                };
                ctx.RatingRanks.Add(rank);
            }
            rating = new Rating {
                VikingId = viking.Id,
                Rank = rank
            };
            ctx.Ratings.Add(rating);
        } else {
            rank = rating.Rank;
        }

        rating.Value = value;
        rank.RatingAverage = 0;
        foreach (Rating r in rank.Ratings) {
            rank.RatingAverage += (float)((decimal)r.Value / (decimal)rank.Ratings.Count);
        }

        if (eID != -1 || uID != null) { // do not sort "single item" (eID == -1 and no uID) category
            RatingRank[] ranks = ctx.RatingRanks
                .Where(rr => rr != rank && rr.CategoryID == category) // Only rank by category.
                .OrderBy(rr => rr.Rank)
                .ToArray();
            bool resortOthers = false;
            rank.Rank = 1; // Start here, work way down.
            for (int i=0;i<ranks.Length;i++) {
                if (!resortOthers && ranks[i].RatingAverage < rank.RatingAverage) {
                    rank.Rank = i+1;
                    resortOthers = true;
                }
                if (resortOthers) ranks[i].Rank = i+2;
                else ranks[i].Rank = i+1;
            }
            if (!resortOthers) rank.Rank = ranks.Length+1;
        }

        rating.Date = DateTime.UtcNow;
        rank.UpdateDate = rating.Date;
        ctx.SaveChanges();

        return new RatingInfo() {
            Id = rating.Id,
            OwnerUid = viking.Uid,
            CategoryID = category,
            RatedEntityID = eID,
            Value = value,
            Date = rating.Date
        };
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/SetRating")]
    [VikingSession]
    public IActionResult SetRating(Viking viking, [FromForm] int categoryID, [FromForm] int ratedEntityID, [FromForm] int ratedValue) {
        return Ok(SetRating(viking, categoryID, ratedEntityID, null, ratedValue));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/SetUserRating")]
    [VikingSession]
    public IActionResult SetUserRating(Viking viking, [FromForm] int categoryID, [FromForm] string ratedUserID, [FromForm] int ratedValue) {
        return Ok(SetRating(viking, categoryID, null, ratedUserID, ratedValue));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetRatingByRatedEntity")]
    [VikingSession]
    public RatingInfo[] GetRatingByRatedEntity(Viking viking, [FromForm] int categoryID, [FromForm] int ratedEntityID) {
        return ctx.Ratings
            .Where(r => r.Viking == viking && r.Rank.CategoryID == categoryID && r.Rank.RatedEntityID == ratedEntityID && r.Rank.RatedUserID == null)
            .Select(r => new RatingInfo {
                    Id = r.Id,
                    OwnerUid = r.Viking.Uid,
                    CategoryID = r.Rank.CategoryID,
                    RatedEntityID = r.Rank.RatedEntityID,
                    Value = r.Value,
                    Date = r.Date
                }
            ).ToArray();
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetTopRatedByCategoryID")]
    public RatingRankInfo[] GetTopRatedByCategoryID([FromForm] int categoryID, [FromForm] int numberOfRecord) {
        return ctx.RatingRanks
            .Where(rr => categoryID == rr.CategoryID)
            .Take(numberOfRecord)
            .Select(rr => new RatingRankInfo(rr))
            .ToArray();
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetTopRatedUserByCategoryID")]
    public IActionResult GetTopRatedUserByCategoryID([FromForm] int categoryID, [FromForm] int numberOfRecord) {
        return Ok(new ArrayOfUserRatingRankInfo {
            UserRatingRankInfo = ctx.RatingRanks
                .Where(rr => rr.RatedUserID != null && (categoryID == rr.CategoryID
                    || (categoryID == 4 && rr.CategoryID == 5) // The party board searches for 4 but the pod rating is set in 5.
                ))
                .OrderBy(rr => rr.Rank)
                .Take(numberOfRecord)
                .Select(rr => new UserRatingRankInfo { RankInfo = new RatingRankInfo(rr), RatedUserID = new Guid(rr.RatedUserID) })
                .ToArray()
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetEntityRatedRank")]
    public IActionResult GetEntityRatedRank([FromForm] int categoryID, [FromForm] int ratedEntityID) {
        // TODO: Add a shortcut here for shipwreck lagoon tracks.
        RatingRank? rank = ctx.RatingRanks.FirstOrDefault(rr => categoryID == rr.CategoryID && rr.RatedEntityID == ratedEntityID);
        if (rank == null) return Ok();
        return Ok(new RatingRankInfo(rank));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetRatingForRatedUser")]
    [VikingSession]
    public IActionResult GetRatingForRatedUser(Viking viking, [FromForm] int categoryID, [FromForm] string ratedUserID) {
        Rating? rating = ctx.Ratings.FirstOrDefault(
            r => r.Viking == viking && categoryID == r.Rank.CategoryID && r.Rank.RatedEntityID == null && r.Rank.RatedUserID == ratedUserID
        );
        return Ok(rating?.Value ?? 0);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetRatingForRatedEntity")]
    [VikingSession]
    public IActionResult GetRatingForRatedEntity(Viking viking, [FromForm] int categoryID, [FromForm] int ratedEntityID) {
        Rating? rating = ctx.Ratings.FirstOrDefault(
            r => r.Viking == viking && categoryID == r.Rank.CategoryID && r.Rank.RatedEntityID == ratedEntityID && r.Rank.RatedUserID == null
        );
        return Ok(rating?.Value ?? 0);
    }
}
