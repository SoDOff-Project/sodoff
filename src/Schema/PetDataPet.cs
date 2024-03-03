using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PetDataPet", Namespace = "")]
[Serializable]
public class PetDataPet {
	public string Geometry;

	public string Texture;

	public string Type;

	public string Name;

	public float Dirtiness;

	public string AccessoryGeometry;

	public string AccessoryTexture;
}
