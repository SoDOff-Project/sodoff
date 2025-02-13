using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "UserBadge", Namespace = "", IsNullable = false)]
    [Serializable]
    public class UserBadge
    {
        [XmlElement(ElementName = "BadgeId")]
        public int[] BadgeId;
    }
}
