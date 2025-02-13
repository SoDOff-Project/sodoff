using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "BadgeDataBadge", Namespace = "")]
    [Serializable]
    public class BadgeDataBadge
    {
        [XmlElement(ElementName = "BadgeId")]
        public int BadgeId;

        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "Description")]
        public string Description;

        [XmlElement(ElementName = "Experience")]
        public int Experience;

        [XmlElement(ElementName = "Pieces")]
        public int Pieces;

        [XmlElement(ElementName = "Mask")]
        public string Mask;

        [XmlElement(ElementName = "Color")]
        public string Color;

        [XmlElement(ElementName = "Grey")]
        public string Grey;

        [XmlElement(ElementName = "PieceDialog", IsNullable = true)]
        public BadgeDataBadgePieceDialog PieceDialog;

        [XmlElement(ElementName = "CompleteDialog", IsNullable = true)]
        public BadgeDataBadgeCompleteDialog CompleteDialog;
    }
}