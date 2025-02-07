using sodoff.Model;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RatingRankInfo", Namespace = "")]
[Serializable]
public class RatingRankInfo {

    public RatingRankInfo() {}
    public RatingRankInfo(RatingRank rank) {
        Id = rank.Id;
        CategoryID = rank.CategoryID;
        RatedEntityID = rank.RatedEntityID??0;
        Rank = rank.Rank;
        RatingAverage = rank.RatingAverage;
        TotalVotes = rank.Ratings.Count;
        UpdateDate = rank.UpdateDate;
    }

    [XmlElement(ElementName = "ID")]
    public int Id;

    [XmlElement(ElementName = "CID")]
    public int CategoryID;

    [XmlElement(ElementName = "EID")]
    public int? RatedEntityID;

    [XmlElement(ElementName = "R")]
    public int Rank;

    [XmlElement(ElementName = "RA")]
    public float RatingAverage;

    [XmlElement(ElementName = "TV")]
    public int TotalVotes;

    [XmlElement(ElementName = "UD")]
    public DateTime UpdateDate;
}