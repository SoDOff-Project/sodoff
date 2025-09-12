using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PrerequisiteItem", Namespace = "")]
[Serializable]
public class PrerequisiteItem {
    public PrerequisiteItem() { }

    public PrerequisiteItem(PrerequisiteItem other) {
        Type = other.Type;
        Value = other.Value;
        Quantity = other.Quantity;
        ClientRule = other.ClientRule;
    }

    [XmlElement(ElementName = "Type")]
    public PrerequisiteRequiredType Type;

    [XmlElement(ElementName = "Value")]
    public string Value;

    [XmlElement(ElementName = "Quantity")]
    public short Quantity;

    [XmlElement(ElementName = "ClientRule")]
    public bool ClientRule;
}
