using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfGamePlayData", Namespace = "")]
[Serializable]
public class ArrayOfGamePlayData
{
	[XmlElement(ElementName = "GamePlayData", IsNullable = true)]
	public GamePlayData[]? GamePlayData;
}
