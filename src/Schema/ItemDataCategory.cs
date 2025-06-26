using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IC", Namespace = "")]
[Serializable]
public class ItemDataCategory {
    public ItemDataCategory() {}

    public ItemDataCategory(ItemDataCategory other) {
        CategoryId = other.CategoryId;
        CategoryName = other.CategoryName;
        IconName = other.IconName;
    }

    [XmlElement(ElementName = "cid")]
	public int CategoryId;

	[XmlElement(ElementName = "cn")]
	public string CategoryName;

	[XmlElement(ElementName = "i", IsNullable = true)]
	public string IconName;
}
