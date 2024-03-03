using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
[XmlRoot(ElementName = "DisplayNames", Namespace = "")]
public class DisplayNameList : List<DisplayName> {
}
public class DisplayName {
	[XmlElement("ID")]
	public int Id;

    [XmlElement("Name")]
	public string Name;
}
