using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AuthorizeJoinRequest", IsNullable = true)]
[Serializable]
public class AuthorizeJoinRequest {
    [XmlElement(ElementName = "Approved")]
    public bool Approved;

    [XmlElement(ElementName = "JoineeID")]
    public string JoineeID;

    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;

    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;
}
