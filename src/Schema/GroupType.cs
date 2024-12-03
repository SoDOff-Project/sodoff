using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GroupType")]
[Serializable]
public enum GroupType {
	[XmlEnum("0")]
	None = 0,
	[XmlEnum("1")]
	System = 1,
	[XmlEnum("2")]
	Public = 2,
	[XmlEnum("3")]
	MembersOnly = 3,
	[XmlEnum("4")]
	Private = 4,
	[XmlEnum("5")]
	Others = 5
}
