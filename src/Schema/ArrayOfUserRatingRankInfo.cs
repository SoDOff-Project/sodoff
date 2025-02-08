using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfUserRatingRankInfo", Namespace = "")]
[Serializable]
public class ArrayOfUserRatingRankInfo {
    [XmlElement(ElementName = "UserRatingRankInfo")]
    public UserRatingRankInfo[] UserRatingRankInfo;
}
