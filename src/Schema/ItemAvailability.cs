using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Availability", Namespace = "")]
[Serializable]
public class ItemAvailability {
    public ItemAvailability() { }

    public ItemAvailability(ItemAvailability other) {
        StartDate = other.StartDate;
        EndDate = other.EndDate;
    }

    [XmlElement(ElementName = "sdate", IsNullable = true)]
    public DateTime? StartDate;

    [XmlElement(ElementName = "edate", IsNullable = true)]
    public DateTime? EndDate;
}
