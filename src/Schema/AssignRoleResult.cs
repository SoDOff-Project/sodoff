using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AssignRoleResult", IsNullable = true)]
[Serializable]
public class AssignRoleResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "InitiatorNewRole", IsNullable = true)]
    public UserRole? InitiatorNewRole;

    [XmlElement(ElementName = "Status")]
    public AssignRoleStatus Status;
}
