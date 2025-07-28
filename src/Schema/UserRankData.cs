using System.Diagnostics;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UserRankData", Namespace = "")]
[Serializable]
public class UserRankData {
    [XmlElement(ElementName = "UserID")]
    public Guid UserID;

    [XmlElement(ElementName = "Points")]
    public int Points;

    [XmlElement(ElementName = "CurrentRank")]
    public UserRank CurrentRank;

    [XmlElement(ElementName = "MemberRank")]
    public UserRank MemberRank;

    [XmlElement(ElementName = "NextRank")]
    public UserRank NextRank;
}
