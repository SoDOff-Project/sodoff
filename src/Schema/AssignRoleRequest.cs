using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AssignRoleRequest", IsNullable = true)]
[Serializable]
public class AssignRoleRequest {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "MemberID")]
    public string MemberID;

    [XmlElement(ElementName = "NewRole")]
    public UserRole NewRole;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;
    
    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;
}
