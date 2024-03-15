using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MMOConfig", Namespace = "")]
[Serializable]
public class MMOConfig {
    [XmlElement(ElementName = "Version")]
    public MMOConfigEntry[] Versions;
}
