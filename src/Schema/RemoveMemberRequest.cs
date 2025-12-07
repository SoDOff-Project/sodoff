using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RemoveMemberRequest", IsNullable = true)]
[Serializable]
public class RemoveMemberRequest {
    [XmlElement(ElementName = "RemoveUserID")]
    public string RemoveUserID;

    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;

    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;
}
