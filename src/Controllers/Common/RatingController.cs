using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class RatingController : Controller
{
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

}
