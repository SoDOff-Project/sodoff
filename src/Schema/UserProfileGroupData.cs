using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UPGD", IsNullable = true, Namespace = "")]
public class UserProfileGroupData
{
	[XmlElement(ElementName = "GroupID")]
	public string GroupID;

	[XmlElement(ElementName = "RoleID", IsNullable = true)]
	public UserRole? RoleID;

	[XmlElement(ElementName = "TypeID", IsNullable = true)]
	public int? TypeID;

	[XmlElement(ElementName = "N")]
	public string Name;

	[XmlElement(ElementName = "L")]
	public string Logo;

	[XmlElement(ElementName = "C")]
	public string Color;
}
