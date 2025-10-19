using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PendingJoinRequest", IsNullable = true)]
[Serializable]
public class PendingJoinRequest {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "FromUserID")]
    public string FromUserID;

    [XmlElement(ElementName = "Message")]
    public string Message;
    
    [XmlElement(ElementName = "StatusID", IsNullable = true)]
    public GroupJoinRequestStatus? StatusID;
}
