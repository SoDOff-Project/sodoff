using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "EditGroupResult", IsNullable = true)]
[Serializable]
public class EditGroupResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "Status")]
    public EditGroupStatus Status;

    [XmlElement(ElementName = "NewRolePermissions", IsNullable = true)]
    public List<RolePermission> NewRolePermissions;
}
