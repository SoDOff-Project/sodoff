using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GamePlayData", Namespace = "")]
[Serializable]
public class GamePlayData
{
	[XmlElement(ElementName = "GMID")] // Game ID
	public int GMID;

	[XmlElement(ElementName = "PLCT")] // presumably Player Count
	public int PLCT;
}
