using System.Xml.Serialization;

[XmlRoot(ElementName = "World", Namespace = "")]
[Serializable]
public class World
{
    [XmlElement(ElementName = "Scene")]
    public string Scene;

    [XmlElement(ElementName = "ID")]
    public int ID;
}
