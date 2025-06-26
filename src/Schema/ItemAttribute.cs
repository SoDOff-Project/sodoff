using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AT", Namespace = "")]
[Serializable]
public class ItemAttribute {
    public ItemAttribute() {}

    public ItemAttribute(ItemAttribute other) {
        Key = other.Key;
        Value = other.Value;
    }

    [XmlElement(ElementName = "k")]
	public string Key;

	[XmlElement(ElementName = "v")]
	public string Value;
}
