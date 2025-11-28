using System.Numerics;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlInclude(typeof(UserItemPositionSetRequest))]
[XmlRoot(ElementName = "UIP", Namespace = "")]
[Serializable]
public class UserItemPosition {
    [XmlElement(ElementName = "iid")]
    public int? ItemID { get; set; }

    [XmlElement(ElementName = "uses")]
    public int? Uses { get; set; }

    [XmlElement(ElementName = "invmdate")]
    public DateTime InvLastModifiedDate { get; set; }

    [XmlElement(ElementName = "uis")]
    public UserItemState UserItemState { get; set; }

    [XmlElement(ElementName = "uia", IsNullable = true)]
    public PairData UserItemAttributes { get; set; }

    [XmlElement(ElementName = "uiss", IsNullable = true)]
    public UserItemStat UserItemStat { get; set; }

    [XmlElement(ElementName = "id", IsNullable = true)]
    public int? UserItemPositionID;

    [XmlElement(ElementName = "uicid")]
    public int? UserInventoryCommonID;

    [XmlElement(ElementName = "i", IsNullable = true)]
    public ItemData? Item;

    [XmlElement(ElementName = "px")]
    public double? PositionX;

    [XmlElement(ElementName = "py")]
    public double? PositionY;

    [XmlElement(ElementName = "pz")]
    public double? PositionZ;

    [XmlElement(ElementName = "rx")]
    public double? RotationX;

    [XmlElement(ElementName = "ry")]
    public double? RotationY;

    [XmlElement(ElementName = "rz")]
    public double? RotationZ;

    [XmlElement(ElementName = "pid", IsNullable = true)]
    public int? ParentID;
}
