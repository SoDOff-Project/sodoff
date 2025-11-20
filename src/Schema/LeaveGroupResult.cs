using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "LeaveGroupResult", IsNullable = true)]
[Serializable]
public class LeaveGroupResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "Status")]
    public LeaveGroupStatus Status;
}
