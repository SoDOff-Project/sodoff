using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PetData", Namespace = "")]
[Serializable]
public class PetData {
	[XmlElement(ElementName = "Pet")]
	public PetDataPet[]? Pet;
}
