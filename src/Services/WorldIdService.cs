using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services;

public class WorldIdService {
    Dictionary<string, int> worlds_id = new();

    public WorldIdService()
    {
        var worlds = XmlUtil.DeserializeXml<World[]>(XmlUtil.ReadResourceXmlString("worlds"));
        foreach (var w in worlds)
        {
            worlds_id[w.Scene] = w.ID;
        }
    }

    public int GetWorldID(string mapName)
    {
        if (worlds_id.ContainsKey(mapName))
            return worlds_id[mapName];
        else
            return 0;
    }
}
