using sodoff.Schema;
using sodoff.Util;
using sodoff.Configuration;
using Microsoft.Extensions.Options;

namespace sodoff.Services;

public class XmlDataService {
    Dictionary<int, string> displayNames = new();
    Dictionary<string, int> worlds_id = new();

    public XmlDataService(IOptions<ApiServerConfig> config) {
        if (!config.Value.LoadNonSoDData)
            return;

        var displayNamesList = XmlUtil.DeserializeXml<DisplayNameList>(XmlUtil.ReadResourceXmlString("displaynames"));
        displayNames.Add(0, "");
        foreach (var n in displayNamesList) {
            displayNames.Add(n.Id, n.Name);
        }

        var worlds = XmlUtil.DeserializeXml<World[]>(XmlUtil.ReadResourceXmlString("worlds"));
        foreach (var w in worlds) {
            worlds_id[w.Scene] = w.ID;
        }
    }

    public string GetDisplayName(int firstNameID, int secondNameID, int thirdNameID) {
        return displayNames[firstNameID] + " " + displayNames[secondNameID] + displayNames[thirdNameID];
    }

    public int GetWorldID(string mapName) {
        if (worlds_id.ContainsKey(mapName))
            return worlds_id[mapName];
        return 0;
    }
}
