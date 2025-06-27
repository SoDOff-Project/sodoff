using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IRO", Namespace = "")]
[Serializable]
public class ItemDataRollover {
    public ItemDataRollover() { }

    public ItemDataRollover(ItemDataRollover other) {
        DialogName = other.DialogName;
        Bundle = other.Bundle;
    }

    [XmlElement(ElementName = "d")]
    public string DialogName;

    [XmlElement(ElementName = "b")]
    public string Bundle;
}
