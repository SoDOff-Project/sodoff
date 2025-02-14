using System.Xml.Serialization;

namespace sodoff.Schema
{
    [Serializable]
    public class CombinedListMessage
    {
        [XmlElement(ElementName = "MessageType")]
        public int MessageType;

        [XmlElement(ElementName = "MessageDate")]
        public DateTime MessageDate;

        [XmlElement(ElementName = "Body", IsNullable = true)]
        public string MessageBody;
    }
}