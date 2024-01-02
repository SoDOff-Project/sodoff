using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ItemStoreController : Controller {

    private readonly DBContext ctx;
    private StoreService storeService;
    private ItemService itemService;

    public ItemStoreController(DBContext ctx, StoreService storeService, ItemService itemService) {
        this.ctx = ctx;
        this.storeService = storeService;
        this.itemService = itemService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetStore")]
    public IActionResult GetStore([FromForm] string getStoreRequest) {
        GetStoreRequest request = XmlUtil.DeserializeXml<GetStoreRequest>(getStoreRequest);

        ItemsInStoreData[] stores = new ItemsInStoreData[request.StoreIDs.Length];
        for (int i = 0; i < request.StoreIDs.Length; i++) {
            stores[i] = storeService.GetStore(request.StoreIDs[i]);
        }

        GetStoreResponse response = new GetStoreResponse {
            Stores = stores
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetItem")]
    public IActionResult GetItem([FromForm] int itemId) {
        if (itemId == 0) // For a null item, return an empty item
            return Ok(new ItemData());
        return Ok(itemService.GetItem(itemId));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetItemsInStore")] // used by World Of Jumpstart
    public IActionResult GetItemsInStore([FromForm] int storeId, [FromForm] string apiKey) {
        return Ok(storeService.GetStore(storeId, ClientVersion.GetVersion(apiKey)));
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetRankAttributeData")]
    public IActionResult GetRankAttributeData([FromForm] string apiKey) {
        uint gameVersion = ClientVersion.GetVersion(apiKey);
        if (gameVersion == ClientVersion.MB)
            return Ok(XmlUtil.ReadResourceXmlString("rankattrib_mb"));
        // TODO, this is a placeholder
        return Ok(XmlUtil.ReadResourceXmlString("rankattrib"));
    }

    [Route("ItemStoreWebService.asmx/GetAssetVersions")]
    public IActionResult GetAssetVersions() {
        // TODO return AssetVersion[]
        return Ok(null);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetAnnouncementsByUser")]
    //[VikingSession(UseLock=false)]
    public IActionResult GetAnnouncements([FromForm] string apiKey, [FromForm] int worldObjectID) {
        // TODO: This is a placeholder, although this endpoint seems to be only used to send announcements to the user (such as the server shutdown), so this might be sufficient.

        uint gameVersion = ClientVersion.GetVersion(apiKey);
        if (gameVersion <= ClientVersion.Max_OldJS && (gameVersion & ClientVersion.WoJS) != 0) {
            return Ok(XmlUtil.DeserializeXml<AnnouncementList>(XmlUtil.ReadResourceXmlString("announcements_wojs")));
        } else if (gameVersion == ClientVersion.SS && worldObjectID == 6) {
            return Ok(XmlUtil.DeserializeXml<AnnouncementList>(XmlUtil.ReadResourceXmlString("announcements_ss")));
        }
        
        return Ok(new AnnouncementList());
    }
}
