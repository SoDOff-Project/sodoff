using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "URRI", Namespace = "")]
[Serializable]
public class UserRatingRankInfo {
    [XmlElement(ElementName = "RI")]
    public RatingRankInfo RankInfo;

    [XmlElement(ElementName = "RUID")]
    public Guid RatedUserID;
}
