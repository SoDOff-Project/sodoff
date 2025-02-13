using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskObjectivePhoto", Namespace = "")]
    [Serializable]
    public class StepTaskObjectivePhoto
    {
        [XmlElement(ElementName = "ItemName")]
        public string[] ItemName;

        [XmlElement(ElementName = "NPC")]
        public string[] NPC;

        [XmlElement(ElementName = "CategoryID")]
        public int[] CategoryID;

        [XmlElement(ElementName = "AttributeID")]
        public int[] AttributeID;

        [XmlElement(ElementName = "Quantity")]
        public int Quantity;
    }
}