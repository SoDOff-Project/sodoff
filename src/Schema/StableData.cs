using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "StableData", Namespace = "")]
[Serializable]
public class StableData {
	[XmlElement(ElementName = "Name")]
	public string Name;

	[XmlElement(ElementName = "ID")]
	public int ID;

	[XmlElement(ElementName = "ItemID")]
	public int ItemID;

	[XmlElement(ElementName = "InventoryID")]
	public int InventoryID;

	[XmlElement(ElementName = "Nests")]
	public List<NestData> NestList;
}
