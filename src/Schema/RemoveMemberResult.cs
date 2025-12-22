using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RemoveMemberResult", IsNullable = true)]
[Serializable]
public class RemoveMemberResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "Status")]
    public RemoveMemberStatus Status;
}
