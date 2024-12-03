using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
[XmlRoot(ElementName = "GP", IsNullable = true)]
public class Group {
	[XmlElement(ElementName = "G", IsNullable = false)]
	public string GroupID;

	[XmlElement(ElementName = "N", IsNullable = false)]
	public string Name;

	[XmlElement(ElementName = "D", IsNullable = false)]
	public string Description;

	[XmlElement(ElementName = "T", IsNullable = false)]
	public GroupType Type;

	[XmlElement(ElementName = "O", IsNullable = true)]
	public string OwnerID;

	[XmlElement(ElementName = "L", IsNullable = true)]
	public string Logo;

	[XmlElement(ElementName = "C", IsNullable = true)]
	public string Color;

	[XmlElement(ElementName = "M", IsNullable = true)]
	public int? MemberLimit;

	[XmlElement(ElementName = "TC", IsNullable = true)]
	public int? TotalMemberCount;

	[XmlElement(ElementName = "A", IsNullable = false)]
	public bool Active;

	[XmlElement(ElementName = "P", IsNullable = true)]
	public string ParentGroupID;

	[XmlElement(ElementName = "PS", IsNullable = true)]
	public int? Points;

	[XmlElement(ElementName = "RK", IsNullable = true)]
	public int? Rank;

	[XmlElement(ElementName = "RT", IsNullable = true)]
	public int? RankTrend;

	[XmlElement(ElementName = "CD")]
	public DateTime CreateDate;
}
