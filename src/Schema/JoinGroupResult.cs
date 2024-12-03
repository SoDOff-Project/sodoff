using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "JoinGroupResult", IsNullable = true, Namespace = "")]
public class JoinGroupResult {
	[XmlElement(ElementName = "GroupStatus")]
	public GroupMembershipStatus GroupStatus;
}
