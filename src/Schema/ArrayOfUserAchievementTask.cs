using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfUserAchievementTask", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfUserAchievementTask
{
	[XmlElement(ElementName = "UserAchievementTask")]
	public UserAchievementTask[] UserAchievementTask;
}
