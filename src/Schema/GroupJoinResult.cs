using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GroupJoinResult", IsNullable = true)]
[Serializable]
public class GroupJoinResult {
    // Token: 0x040009C3 RID: 2499
    [XmlElement(ElementName = "Success")]
    public bool Success;

    // Token: 0x040009C4 RID: 2500
    [XmlElement(ElementName = "Status")]
    public JoinGroupStatus Status;
}