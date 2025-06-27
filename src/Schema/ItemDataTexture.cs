using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IT", Namespace = "")]
[Serializable]
public class ItemDataTexture {
    public ItemDataTexture() { }

    public ItemDataTexture(ItemDataTexture other) {
        TextureName = other.TextureName;
        TextureTypeName = other.TextureTypeName;
        OffsetX = other.OffsetX;
        OffsetY = other.OffsetY;
    }

    [XmlElement(ElementName = "n")]
    public string TextureName;

    [XmlElement(ElementName = "t")]
    public string TextureTypeName;

    [XmlElement(ElementName = "x", IsNullable = true)]
    public float? OffsetX;

    [XmlElement(ElementName = "y", IsNullable = true)]
    public float? OffsetY;
}
