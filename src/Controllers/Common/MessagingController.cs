using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;

namespace sodoff.Controllers.Common;
public class MessagingController : Controller {

    public readonly ModerationService moderationService;
    public readonly MessagingService messagingService;
    public readonly DBContext ctx;

    public MessagingController(ModerationService moderationService, MessagingService messagingService, DBContext ctx)
    {
        this.moderationService = moderationService;
        this.messagingService = messagingService;
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MessagingWebService.asmx/GetUserMessageQueue")]
    [VikingSession]
    public ArrayOfMessageInfo? GetUserMessageQueue(Viking viking) {
        return new ArrayOfMessageInfo { MessageInfo = messagingService.ConstructUserMessageInfoArray(viking, false, false) };
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MessagingWebService.asmx/SendMessage")]
    public IActionResult SendMessage() {
        // TODO: this is a placeholder
        return Ok(false);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MessagingWebService.asmx/SaveMessage")]
    public IActionResult SaveMessage() {
        // TODO: this is a placeholder
        return Ok(false);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MessageWebService.asmx/GetCombinedListMessage")]
    public ArrayOfCombinedListMessage? GetCombinedListMessage([FromForm] Guid userId)
    {
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == userId);

        if (viking == null) return new ArrayOfCombinedListMessage();

        return new ArrayOfCombinedListMessage { CombinedListMessage = messagingService.ConstructCombinedMessageArray(viking) };
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ChatWebService.asmx/ReportUser")]
    [VikingSession]
    public IActionResult ReportUser(Viking viking, [FromForm] string apiToken, [FromForm] Guid reportUserID, [FromForm] int reportReason)
    {
        // find viking
        Viking? vikingToReport = ctx.Vikings.FirstOrDefault(e => e.Uid == reportUserID);

        if (vikingToReport != null)
        {
            Report reportFiled = moderationService.AddReportToViking(apiToken, viking, vikingToReport, (ReportType)reportReason);
            if (reportFiled != null) return Ok(true);
            else return Unauthorized();
        } else return Unauthorized();
    }
}
