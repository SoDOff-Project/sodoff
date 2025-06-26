using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "STAT", Namespace = "", IsNullable = false)]
[Serializable]
public class Stat {
    public Stat() {}

    public Stat(Stat other) {
        ItemID = other.ItemID;
        ItemStatsID = other.ItemStatsID;
        SetID = other.SetID;
        Probability = other.Probability;
        ItemStatsRangeMaps = other.ItemStatsRangeMaps.Select(s => new StatRangeMap(s)).ToList();
    }

    [XmlElement(ElementName = "IID", IsNullable = false)]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "ISID", IsNullable = false)]
	public int ItemStatsID { get; set; }

	[XmlElement(ElementName = "SID", IsNullable = false)]
	public int SetID { get; set; }

	[XmlElement(ElementName = "PROB", IsNullable = false)]
	public int Probability { get; set; }

	[XmlElement(ElementName = "ISRM", IsNullable = false)]
	public List<StatRangeMap> ItemStatsRangeMaps { get; set; }
}
