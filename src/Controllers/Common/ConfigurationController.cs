using Microsoft.AspNetCore.Mvc;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class ConfigurationController : Controller {

    [HttpPost]
    //[Produces("application/xml")]
    [Route("ConfigurationWebService.asmx/GetMMOServerInfoWithZone")]
    public IActionResult GetMMOServerInfoWithZone([FromForm] string apiKey) {
        // TODO: this is a placeholder
        if (apiKey == "A1A13A0A-7C6E-4E9B-B0F7-22034D799013" || apiKey == "A2A09A0A-7C6E-4E9B-B0F7-22034D799013" || apiKey == "A3A12A0A-7C6E-4E9B-B0F7-22034D799013") { // NOTE: in this request apiKey is send uppercase
            // do not send MMO servers to old (incompatibility with MMO server) client
            return Ok(XmlUtil.SerializeXml(new MMOServerInformation[0]));
        }
        return Ok(XmlUtil.ReadResourceXmlString("mmo"));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ConfigurationWebService.asmx/GetContentByTypeByUser")] // used by World Of Jumpstart
    public IActionResult GetContentByTypeByUser([FromForm] int contentType)
    {
        if (contentType == 1) return Ok(new ContentInfo
        {
            ContentInfoArray = XmlUtil.DeserializeXml<ContentInfoData[]>(XmlUtil.ReadResourceXmlString("content_jukebox"))
        });
        if (contentType == 2) return Ok(new ContentInfo
        {
            ContentInfoArray = XmlUtil.DeserializeXml<ContentInfoData[]>(XmlUtil.ReadResourceXmlString("content_movie"))
        });
        if (contentType == 3) return Ok(new ContentInfo
        {
            ContentInfoArray = XmlUtil.DeserializeXml<ContentInfoData[]>(XmlUtil.ReadResourceXmlString("content_arcade"))
        });
        if (contentType == 4) return Ok(new ContentInfo
        {
            ContentInfoArray = XmlUtil.DeserializeXml<ContentInfoData[]>(XmlUtil.ReadResourceXmlString("content_learning"))
        });

        return NotFound();
    }
}
