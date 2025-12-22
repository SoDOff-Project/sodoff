using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "JoinGroupRequest", IsNullable = true)]
[Serializable]
public class JoinGroupRequest {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;

    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;

    [XmlElement(ElementName = "Message")]
    public string Message;

    [XmlElement(ElementName = "JoinRequestStatus")]
    public GroupJoinRequestStatus? JoinRequestStatus;

    [XmlElement(ElementName = "JoinByInvite")]
    public bool JoinByInvite;

    [XmlElement(ElementName = "InviterID")]
    public string InviterID;
}
