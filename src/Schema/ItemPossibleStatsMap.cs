using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IPSM", Namespace = "", IsNullable = false)]
[Serializable]
public class ItemPossibleStatsMap {
    public ItemPossibleStatsMap() { }

    public ItemPossibleStatsMap(ItemPossibleStatsMap other) {
        ItemID = other.ItemID;
        ItemStatsCount = other.ItemStatsCount;
        SetID = other.SetID;
        Stats = other.Stats.Select(s => new Stat(s)).ToList();
    }

    [XmlElement(ElementName = "IID", IsNullable = false)]
    public int ItemID { get; set; }

    [XmlElement(ElementName = "SC", IsNullable = false)]
    public int ItemStatsCount { get; set; }

    [XmlElement(ElementName = "SID", IsNullable = false)]
    public int SetID { get; set; }

    [XmlElement(ElementName = "SS", IsNullable = false)]
    public List<Stat> Stats { get; set; }
}
