using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "progress", Namespace = "")]
[Serializable]
public class GameProgress {
	[XmlElement(ElementName = "name")]
	public string Name;

	[XmlElement(ElementName = "version")]
	public string Version;

	[XmlElement(ElementName = "chapter")]
	public string[] Chapter;

	[XmlElement(ElementName = "custom")]
	public string? Custom;
}

