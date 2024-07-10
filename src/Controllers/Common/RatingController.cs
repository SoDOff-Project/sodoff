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
        // TODO: better calculations, improve module determination code
        // for now, a trusty placeholder

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
