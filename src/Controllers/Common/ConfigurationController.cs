using Microsoft.AspNetCore.Mvc;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class ConfigurationController : Controller {
    private MMOConfigService mmoConfigService;

    public ConfigurationController(MMOConfigService mmoConfigService) {
        this.mmoConfigService = mmoConfigService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ConfigurationWebService.asmx/GetMMOServerInfoWithZone")]
    [Route("ConfigurationWebService.asmx/GetMMOServerInfo")] // used by SuperSecret
    public IActionResult GetMMOServerInfoWithZone([FromForm] string apiKey) {
        return Ok(mmoConfigService.GetMMOServerInformation(
            ClientVersion.GetVersion(apiKey.ToLower()) // NOTE: in this request apiKey is send uppercase
        ));
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
        if (contentType == 5) return Ok(new ContentInfo
        {
            ContentInfoArray =XmlUtil.DeserializeXml<ContentInfoData[]>(XmlUtil.ReadResourceXmlString("content_blastermovie"))
        });

        return NotFound();
    }
}
