using System.Xml.Serialization;
using sodoff.Util;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MMOConfigEntry", Namespace = "")]
[Serializable]
public class MMOConfigEntry {
    [XmlElement(ElementName = "VersionMin")]
    public string VersionMin {
        get { return VersionFirst.ToString("X"); }
        set { VersionFirst = XmlUtil.HexToUint(value); }
    }

    [XmlIgnore]
    public uint VersionFirst;

    [XmlElement(ElementName = "VersionMax")]
    public string VersionMax {
        get { return VersionLast.ToString("X"); }
        set { VersionLast = XmlUtil.HexToUint(value); }
    }

    [XmlIgnore]
    public uint VersionLast;

    [XmlElement(ElementName = "ZoneList")]
    public string ZoneList;

    [XmlElement(ElementName = "MMOServerInfo")]
    public MMOServerData[] MMOServerDataArray;
}
