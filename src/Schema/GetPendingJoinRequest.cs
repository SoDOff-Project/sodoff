using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GetPendingJoinRequest", IsNullable = true)]
[Serializable]
public class GetPendingJoinRequest {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;
}
