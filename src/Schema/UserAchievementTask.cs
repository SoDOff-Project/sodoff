using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UAT", Namespace = "")]
[Serializable]
public class UserAchievementTask
{
	[XmlElement(ElementName = "gid")]
	public int AchievementTaskGroupID;

	[XmlElement(ElementName = "aq", IsNullable = true)]
	public int? AchievedQuantity; // current points value

	[XmlElement(ElementName = "nl", IsNullable = true)]
	public int? NextLevel; // next level number

	[XmlElement(ElementName = "qr", IsNullable = true)]
	public int? QuantityRequired; // points need to reach next level

	[XmlElement(ElementName = "ntr", IsNullable = true)]
	public AchievementTaskReward[] NextLevelAchievementRewards;
}
