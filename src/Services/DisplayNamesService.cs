using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services;

public class DisplayNamesService {
    Dictionary<int, string> displayNames = new();

    public DisplayNamesService(ItemService itemService) {
        DisplayNameList displayNamesList = XmlUtil.DeserializeXml<DisplayNameList>(XmlUtil.ReadResourceXmlString("displaynames"));
        foreach (var n in displayNamesList) {
            displayNames.Add(n.Id, n.Name);
        }
    }

    public string GetName(int firstNameID, int secondNameID, int thirdNameID) {
        return displayNames[firstNameID] + " " + displayNames[secondNameID] + displayNames[thirdNameID];
    }
}
