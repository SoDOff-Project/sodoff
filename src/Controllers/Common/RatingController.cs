using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using sodoff.Attributes;
using sodoff.Configuration;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;
using System.Security.Cryptography;

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
    public RatingInfo SubmitRating(Viking viking, int category, int? eID, string? uID, int value) {
        RatingRank? rank;
        Rating? rating = viking.Ratings.FirstOrDefault(
            r => category == r.CategoryID && r.RatedEntityID == eID && r.RatedUserID == uID
        );
        bool newRating = rating == null;
        if (newRating) {
            rating = new Rating {
                OwnerId = viking.Id,
                CategoryID = category,
                RatedEntityID = eID,
                RatedUserID = uID
            };
            ctx.Ratings.Add(rating);
            rank = ctx.RatingRanks.FirstOrDefault(rr => rr.CategoryID == category && rr.RatedEntityID == eID && rr.RatedUserID == uID);
        } else {
            rank = rating.Rank;
        }
        if (rank == null) {
            rank = new RatingRank {
                CategoryID = category,
                RatedEntityID = eID,
                RatedUserID = uID,
                Rank = 0
            };
            ctx.RatingRanks.Add(rank);
        }
        rank.TotalVotes = rank.Ratings?.Count??1;
        if (newRating) {
            rating.Rank = rank;
            rank.TotalVotes++;
        }
        if (rank.Ratings != null) {
            rank.RatingAverage = 0;
            foreach (Rating r in rank.Ratings) {
                if (r == rating) continue;
                rank.RatingAverage += (float)((decimal)r.Value / (decimal)rank.TotalVotes);
            }
            rank.RatingAverage += (float)((decimal)value / (decimal)rank.TotalVotes);
        } else {
            rank.RatingAverage = value;
        }
        if (eID != -1 || uID != null) {
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
        rating.Value = value;
        rating.Date = DateTime.UtcNow;
        rank.UpdateDate = rating.Date;
        ctx.SaveChanges();
        RatingInfo info = new() {
            Id = rating.Id,
            OwnerUid = viking.Uid,
            CategoryID = category,
            RatedEntityID = eID,
            Value = value,
            Date = rating.Date
        };
        return info;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/SetRating")]
    [VikingSession]
    public IActionResult SubmitRating(Viking viking, [FromForm] int categoryID, [FromForm] int ratedEntityID, [FromForm] int ratedValue) {
        return Ok(SubmitRating(viking, categoryID, ratedEntityID, null, ratedValue));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/SetUserRating")]
    [VikingSession]
    public IActionResult SubmitUserRating(Viking viking, [FromForm] int categoryID, [FromForm] string ratedUserID, [FromForm] int ratedValue) {
        return Ok(SubmitRating(viking, categoryID, null, ratedUserID, ratedValue));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetRatingByRatedEntity")]
    public RatingInfo[] GetAllRatings([FromForm] int categoryID, [FromForm] int ratedEntityID) {
        return ctx.Ratings
            .Where(r => r.CategoryID == categoryID && r.RatedEntityID == ratedEntityID && r.RatedUserID == null)
            .Select(r => new RatingInfo {
                    Id = r.Id,
                    OwnerUid = r.Owner.Uid,
                    CategoryID = r.CategoryID,
                    RatedEntityID = r.RatedEntityID,
                    Value = r.Value,
                    Date = r.Date
                }
            ).ToArray();
    }

    // TODO: Implement for shipwreck tracks, maybe.
    // Looking at the code, we actually want to delete all ratings for the entity.
    // This current implementation only deletes the user's ranking, and not thoroughly.
    //[HttpPost]
    //[Route("RatingWebService.asmx/DeleteEntityRating")]
    //[VikingSession]
    public IActionResult ClearRating(Viking viking, [FromForm] int categoryID, [FromForm] int ratedEntityID) {
        Rating? rating = viking.Ratings.FirstOrDefault(
            r => categoryID == r.CategoryID && r.RatedEntityID == ratedEntityID && r.RatedUserID == null
        );
        if (rating != null) ctx.Ratings.Remove(rating);
        return Ok();
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetTopRatedByCategoryID")]
    public RatingRankInfo[] GetRanks([FromForm] int categoryID, [FromForm] int numberOfRecord) {
        return ctx.RatingRanks
            .Where(rr => categoryID == rr.CategoryID)
            .Take(numberOfRecord)
            .Select(rr => new RatingRankInfo(rr))
            .ToArray();
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetTopRatedUserByCategoryID")]
    public IActionResult GetUserRanks([FromForm] int categoryID, [FromForm] int numberOfRecord) {
        Console.WriteLine(new ArrayOfUserRatingRankInfo {
            UserRatingRankInfo = ctx.RatingRanks
                .Where(rr => rr.RatedUserID != null && (categoryID == rr.CategoryID
                    || (categoryID == 4 && rr.CategoryID == 5) // The party board searches for 4 but the pod rating is set in 5.
                ))
                .Take(numberOfRecord)
                .Select(rr => new UserRatingRankInfo { RankInfo = new RatingRankInfo(rr) })
                .ToArray()
        });
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
    public IActionResult GetRank([FromForm] int categoryID, [FromForm] int ratedEntityID) {
        // TODO: Add a shortcut here for shipwreck lagoon tracks.
        RatingRank? rank = ctx.RatingRanks.FirstOrDefault(rr => categoryID == rr.CategoryID && rr.RatedEntityID == ratedEntityID);
        if (rank == null) return Ok();
        return Ok(new RatingRankInfo(rank));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetRatingForRatedUser")]
    public IActionResult GetUserRating([FromForm] int categoryID, [FromForm] string ratedUserID) {
        Rating? rating = ctx.Ratings.FirstOrDefault(
            r => categoryID == r.CategoryID && r.RatedEntityID == null && r.RatedUserID == ratedUserID
        );
        return Ok(rating?.Value ?? 0);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("RatingWebService.asmx/GetRatingForRatedEntity")]
    public IActionResult GetRating([FromForm] int categoryID, [FromForm] int ratedEntityID) {
        Rating? rating = ctx.Ratings.FirstOrDefault(
            r => categoryID == r.CategoryID && r.RatedEntityID == ratedEntityID && r.RatedUserID == null
        );
        return Ok(rating?.Value ?? 0);
    }
}
