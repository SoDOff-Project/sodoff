using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "BadgeDataBadgeCompleteDialog", Namespace = "")]
    [Serializable]
    public class BadgeDataBadgeCompleteDialog
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}