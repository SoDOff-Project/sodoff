using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RatingInfo", Namespace = "")]
[Serializable]
public class RatingInfo {
    [XmlElement(ElementName = "ID")]
    public int Id;

    [XmlElement(ElementName = "UID")]
    public Guid OwnerUid;

    [XmlElement(ElementName = "CID")]
    public int CategoryID;

    [XmlElement(ElementName = "EID")]
    public int? RatedEntityID;

    [XmlElement(ElementName = "RV")]
    public int Value;

    [XmlElement(ElementName = "RD")]
    public DateTime Date;
}
