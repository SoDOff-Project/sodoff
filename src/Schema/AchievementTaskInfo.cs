using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AchievementTaskInfo", Namespace = "")]
[Serializable]
public class AchievementTaskInfo
{
	[XmlElement(ElementName = "AchievementInfoID")]
	public int InfoID;
	
	[XmlElement(ElementName = "AchievementTaskID")]
	public int TaskID;
	
	[XmlElement(ElementName = "PointValue")]
	public int PointValue;
	
	[XmlElement(ElementName = "Reproducible")]
	public bool Reproducible;
	
	[XmlElement(ElementName = "AchieventTaskReward")]
	public AchievementReward[] Rewards;
	
	[XmlElement(ElementName = "AchievementName")]
	public string Name;
	
	[XmlElement(ElementName = "AchievementTaskGroupID")]
	public int TaskGroupID;
	
	[XmlElement(ElementName = "Level")]
	public int Level;
}
