using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "BadgeDataBadgePieceDialog", Namespace = "")]
    [Serializable]
    public class BadgeDataBadgePieceDialog
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}