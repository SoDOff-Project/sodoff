using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GetPendingJoinResult", IsNullable = true)]
[Serializable]
public class GetPendingJoinResult {
    // Token: 0x04000A48 RID: 2632
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "Requests")]
    public PendingJoinRequest[] Requests;
}
