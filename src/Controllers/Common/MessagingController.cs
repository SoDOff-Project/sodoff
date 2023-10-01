using Microsoft.AspNetCore.Mvc;
using sodoff.Schema;

namespace sodoff.Controllers.Common;
public class MessagingController : Controller {

    [HttpPost]
    [Produces("application/xml")]
    [Route("MessagingWebService.asmx/GetUserMessageQueue")]
    public ArrayOfMessageInfo? GetUserMessageQueue() {
        // TODO: this is a placeholder
        return null;
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
    public IActionResult GetCombinedListMessage()
    {
        // TODO - placeholder
        return Ok(new ArrayOfMessageInfo());
    }
}
