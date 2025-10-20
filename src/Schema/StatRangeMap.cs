using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "SRM", Namespace = "", IsNullable = false)]
[Serializable]
public class StatRangeMap {
    public StatRangeMap() { }

    public StatRangeMap(StatRangeMap other) {
        ItemStatsID = other.ItemStatsID;
        ItemStatsName = other.ItemStatsName;
        ItemTierID = other.ItemTierID;
        StartRange = other.StartRange;
        EndRange = other.EndRange;
    }

    [XmlElement(ElementName = "ISID", IsNullable = false)]
    public int ItemStatsID { get; set; }

    [XmlElement(ElementName = "ISN", IsNullable = false)]
    public string ItemStatsName { get; set; }

    [XmlElement(ElementName = "ITID", IsNullable = false)]
    public int ItemTierID { get; set; }

    [XmlElement(ElementName = "SR", IsNullable = false)]
    public int StartRange { get; set; }

    [XmlElement(ElementName = "ER", IsNullable = false)]
    public int EndRange { get; set; }
}
