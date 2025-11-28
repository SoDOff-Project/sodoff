using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AchievementCompletion", Namespace = "")]
[Serializable]
public class AchievementCompletion
{
	[XmlElement(ElementName = "AchievementID")]
	public int AchievementID;
}
