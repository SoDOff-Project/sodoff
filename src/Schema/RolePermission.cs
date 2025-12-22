using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RP", IsNullable = true)]
[Serializable]
public class RolePermission {
    [XmlElement(ElementName = "G")]
    public GroupType GroupType;

    [XmlElement(ElementName = "R")]
    public UserRole Role;

    [XmlElement(ElementName = "P")]
    public List<string> Permissions;
}
