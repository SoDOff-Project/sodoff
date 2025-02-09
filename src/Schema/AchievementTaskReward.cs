using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ATR", Namespace = "")]
[Serializable]
public class AchievementTaskReward
{
	[XmlElement(ElementName = "q")]
	public int RewardQuantity;

	[XmlElement(ElementName = "p")]
	public int PointTypeID;

	[XmlElement(ElementName = "r")]
	public int RewardID;

	[XmlElement(ElementName = "pg")]
	public int ProductGroupID;

	[XmlElement(ElementName = "a")]
	public int AchievementInfoID;

	[XmlElement(ElementName = "ii", IsNullable = true)]
	public int? ItemID;
}
