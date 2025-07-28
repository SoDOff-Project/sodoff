using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "NestData", Namespace = "")]
[Serializable]
public class NestData {
	[XmlElement(ElementName = "PetID")]
	public int PetID;

	[XmlElement(ElementName = "ID")]
	public int ID;
}
