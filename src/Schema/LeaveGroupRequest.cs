using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "LeaveGroupRequest", IsNullable = true)]
[Serializable]
public class LeaveGroupRequest {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;

    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;
}
