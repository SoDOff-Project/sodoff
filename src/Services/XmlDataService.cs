using sodoff.Schema;
using sodoff.Util;
using sodoff.Configuration;
using Microsoft.Extensions.Options;

namespace sodoff.Services;

public class XmlDataService {
    Dictionary<int, string> displayNames = new();
    Dictionary<string, int> worlds_id = new();
    Dictionary<uint, Dictionary<string, PartyInfo>> partyData = new();
    Dictionary<string, string> partyLocations;

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
        
        var parties = XmlUtil.DeserializeXml<PartiesInfo>(XmlUtil.ReadResourceXmlString("parties_info"));
        foreach (PartyInfo party in parties.Parties) {
            if (partyData.TryGetValue(party.GameID, out Dictionary<string, PartyInfo>? partyDict)) {
                partyDict.TryAdd(party.Type, party);
            } else {
                partyDict = new Dictionary<string, PartyInfo> {
                    {party.Type, party}
                };
                partyData.Add(party.GameID, partyDict);
            }
        }

        partyLocations = parties.LocationIcons;
    }

    public string GetDisplayName(int firstNameID, int secondNameID, int thirdNameID) {
        return displayNames[firstNameID] + " " + displayNames[secondNameID] + displayNames[thirdNameID];
    }

    public int GetWorldID(string mapName) {
        if (worlds_id.ContainsKey(mapName))
            return worlds_id[mapName];
        return 0;
    }

    public PartyInfo? GetParty(uint gameID, string partyType) {
        if (!partyData.TryGetValue(gameID, out Dictionary<string, PartyInfo>? partyDict)) return null;
        partyDict.TryGetValue(partyType, out PartyInfo? party);
        return party;
    }

    public string? GetPartyLocation(PartyInfo party) {
        partyLocations.TryGetValue(party.Location, out string? value);
        return value;
    }
}
