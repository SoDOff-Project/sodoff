using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskObjectiveCreative", Namespace = "")]
    [Serializable]
    public class StepTaskObjectiveCreative
    {
        [XmlElement(ElementName = "Type")]
        public int Type;

        [XmlElement(ElementName = "AttributeID")]
        public int[] AttributeID;
    }
}