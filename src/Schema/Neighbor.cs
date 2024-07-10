using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Neighbor", Namespace = "")]
[Serializable]
public class Neighbor
{
    [XmlElement(ElementName = "NeighborUserID")]
    public Guid NeighborUserID;

    [XmlElement(ElementName = "Slot")]
    public int Slot;
}