using System.Xml.Serialization;

namespace sodoff.Schema;

public class BluePrintSpecification {
    public BluePrintSpecification() {}

    public BluePrintSpecification(BluePrintSpecification other) {
        BluePrintSpecID = other.BluePrintSpecID;
        BluePrintItemID = other.BluePrintItemID;
        ItemID = other.ItemID;
        CategoryID = other.CategoryID;
        ItemRarity = other.ItemRarity;
        Tier = other.Tier;
        Quantity = other.Quantity;
        SpecificationType = other.SpecificationType;
    }

    [XmlElement(ElementName = "BPSID", IsNullable = false)]
	public int BluePrintSpecID { get; set; }

	[XmlElement(ElementName = "BPIID", IsNullable = false)]
	public int BluePrintItemID { get; set; }

	public bool ShouldSerializeBluePrintItemID() { return BluePrintItemID != 0; }

	[XmlElement(ElementName = "IID", IsNullable = true)]
	public int? ItemID { get; set; }

	[XmlElement(ElementName = "CID", IsNullable = true)]
	public int? CategoryID { get; set; }

	[XmlElement(ElementName = "IR", IsNullable = true)]
	public ItemRarity? ItemRarity { get; set; }

	[XmlElement(ElementName = "T", IsNullable = true)]
	public ItemTier? Tier { get; set; }

	[XmlElement(ElementName = "QTY", IsNullable = false)]
	public int Quantity { get; set; }

	[XmlElement(ElementName = "ST", IsNullable = true)]
	public SpecificationType? SpecificationType { get; set; }

	public bool ShouldSerializeSpecificationType() { return SpecificationType != null; }
}
