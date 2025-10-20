using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services;

public class PartyService {
    PartiesInfo parties;
    Dictionary<uint, Dictionary<string, PartyInfo>> partyData = new Dictionary<uint, Dictionary<string, PartyInfo>>();

    public PartyService() {
        parties = XmlUtil.DeserializeXml<PartiesInfo>(XmlUtil.ReadResourceXmlString("parties_info"));

        foreach (PartyInfo party in parties.Parties) {
            if (partyData.TryGetValue(party.GameID, out Dictionary<string, PartyInfo>? partyDict)) {
                partyDict.TryAdd(party.Type, party);
            } else {
                partyDict = new Dictionary<string, PartyInfo>();
                partyData.Add(party.GameID, partyDict);
            }
        }
    }

    public PartyInfo? GetParty(uint gameID, string partyType) {
        if (!partyData.TryGetValue(gameID, out Dictionary<string, PartyInfo>? partyDict)) return null;
        partyDict.TryGetValue(partyType, out PartyInfo? party);
        return party;
    }

    public string? GetLocation(PartyInfo party) {
        parties.LocationIcons.TryGetValue(party.Location, out string? value);
        return value;
    }
}