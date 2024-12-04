using System.Xml.Serialization;

namespace sodoff.Schema;

public enum GroupMembershipStatus {
	[XmlEnum("1")]
	APPROVAL_PENDING = 1,
	[XmlEnum("2")]
	APPROVED,
	[XmlEnum("3")]
	REJECTED,
	[XmlEnum("4")]
	SELF_BLOCKED,
	[XmlEnum("5")]
	ALREADY_MEMBER,
	[XmlEnum("6")]
	OTHERS
}
