using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GroupMember", IsNullable = true)]
[Serializable]
public class GroupMember {
    [XmlElement(ElementName = "UserID", IsNullable = false)]
    public string UserID;

    [XmlElement(ElementName = "GroupID", IsNullable = false)]
    public string GroupID;

    [XmlElement(ElementName = "DisplayName", IsNullable = false)]
    public string DisplayName;

    [XmlElement(ElementName = "JoinDate")]
    public DateTime JoinDate;

    [XmlElement(ElementName = "Online")]
    public bool Online;

    [XmlElement(ElementName = "RoleID", IsNullable = true)]
    public int? RoleID;

    [XmlElement(ElementName = "Points", IsNullable = true)]
    public int? Points;

    [XmlElement(ElementName = "Rank", IsNullable = true)]
    public int? Rank;
    
    [XmlElement(ElementName = "RankTrend", IsNullable = true)]
    public int? RankTrend;
}

