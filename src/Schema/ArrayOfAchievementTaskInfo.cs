using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfAchievementTaskInfo", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfAchievementTaskInfo
{
	[XmlElement(ElementName = "AchievementTaskInfo")]
	public AchievementTaskInfo[] AchievementTaskInfo;
}
