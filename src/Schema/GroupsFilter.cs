using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GroupsFilter", IsNullable = true)]
[Serializable]
public class GroupsFilter {
    [XmlElement(ElementName = "GroupType")]
    public GroupType? GroupType;

    [XmlElement(ElementName = "Locale")]
    public string Locale;

    [XmlElement(ElementName = "PointTypeID")]
    public int? PointTypeID;
}
