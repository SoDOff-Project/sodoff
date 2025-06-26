using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IRE", Namespace = "")]
[Serializable]
public class ItemDataRelationship {
    public ItemDataRelationship() {}

    public ItemDataRelationship(ItemDataRelationship other) {
        Type = other.Type;
        ItemId = other.ItemId;
        Weight = other.Weight;
        Quantity = other.Quantity;
        MaxQuantity = other.MaxQuantity;
    }

    [XmlElement(ElementName = "t")]
	public string Type;

	[XmlElement(ElementName = "id")]
	public int ItemId;

	[XmlElement(ElementName = "wt")]
	public int Weight;

	[XmlElement(ElementName = "q")]
	public int Quantity;

	[XmlElement(ElementName = "mxq")]
	public int? MaxQuantity;
}
