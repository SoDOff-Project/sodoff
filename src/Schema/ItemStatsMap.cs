using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ISM", Namespace = "", IsNullable = false)]
[Serializable]
public class ItemStatsMap {
    public ItemStatsMap() { }

    public ItemStatsMap(ItemStatsMap other) {
        ItemID = other.ItemID;
        ItemTier = other.ItemTier;
        ItemStats = other.ItemStats.Select(s => new ItemStat(s)).ToArray();
    }

    [XmlElement(ElementName = "IID", IsNullable = false)]
    public int ItemID { get; set; }

    [XmlElement(ElementName = "IT", IsNullable = false)]
    public ItemTier ItemTier { get; set; }

    [XmlElement(ElementName = "ISS", IsNullable = false)]
    public ItemStat[] ItemStats { get; set; }
}
