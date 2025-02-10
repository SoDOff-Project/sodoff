using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "BadgeData", Namespace = "", IsNullable = true)]
    [Serializable]
    public class BadgeData
    {
        [XmlElement(ElementName = "Badge")]
        public BadgeDataBadge[] Badge;
    }
}
