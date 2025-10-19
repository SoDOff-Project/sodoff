using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AuthorizeJoinResult", IsNullable = true)]
[Serializable]
public class AuthorizeJoinResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;
    
    [XmlElement(ElementName = "Status")]
    public AuthorizeJoinStatus Status;
}
