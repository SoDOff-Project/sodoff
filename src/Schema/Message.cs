using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "Message")]
    [Serializable]
    public class Message
    {
        [XmlElement(ElementName = "MessageID")]
        public int? MessageID;

        [XmlElement(ElementName = "Creator")]
        public string Creator;

        [XmlElement(ElementName = "MessageLevel")]
        public MessageLevel MessageLevel;

        [XmlElement(ElementName = "MessageType")]
        public MessageType MessageType;

        [XmlElement(ElementName = "Content", IsNullable = true)]
        public string? Content;

        [XmlElement(ElementName = "ReplyToMessageID", IsNullable = true)]
        public int? ReplyToMessageID;

        [XmlElement(ElementName = "CreateTime")]
        public DateTime CreateTime;

        [XmlElement(ElementName = "UpdateDate", IsNullable = true)]
        public DateTime? UpdateDate;

        [XmlElement(ElementName = "ConversationID")]
        public int ConversationID;

        [XmlElement(ElementName = "DisplayAttribute", IsNullable = true)]
        public string? DisplayAttribute;
    }
}