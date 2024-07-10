using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "NeighborData", Namespace = "")]
[Serializable]
public class NeighborData
{
    [XmlElement(ElementName = "UserID")]
    public Guid UserID;

    [XmlElement(ElementName = "Neighbors")]
    public Neighbor[] Neighbors;
}