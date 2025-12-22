using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "CreateGroupResult", IsNullable = true)]
[Serializable]
public class CreateGroupResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "Status")]
    public CreateGroupStatus Status;

    [XmlElement(ElementName = "Group")]
    public Group Group;
}