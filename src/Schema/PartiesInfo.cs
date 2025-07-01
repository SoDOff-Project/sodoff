using sodoff.Util;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PartiesInfo", Namespace = "")]
[Serializable]
public class PartiesInfo {
    [XmlElement(ElementName = "Location")]
    public PartyIconData[] IconData {
        get { return LocationIcons.Select(i => new PartyIconData {Name = i.Key, Icon = i.Value}).ToArray(); }
        set { LocationIcons = value.ToDictionary(i => i.Name, i => i.Icon); }
    }

    [XmlIgnore]
    public Dictionary<string, string> LocationIcons;

    [XmlElement(ElementName = "Party")]
    public PartyInfo[] Parties;
}

public class PartyIconData {
    [XmlElement("Name")]
    public string Name;

    [XmlElement("Icon")]
    public string Icon;
}

public class PartyInfo {
    [XmlElement("Version")]
    public string Version {
        get { return GameID.ToString("X"); }
        set { GameID = XmlUtil.HexToUint(value); }
    }

    [XmlIgnore]
    public uint GameID;

    [XmlElement("Location")]
    public string Location;

    [XmlElement("Type")]
    public string Type;

    [XmlElement("Icon")]
    public string Icon;

    [XmlElement("AssetBundle")]
    public string Bundle;

    [XmlElement("Descriptor")]
    public string? Descriptor;
}