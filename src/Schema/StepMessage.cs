using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepMessage", Namespace = "")]
    [Serializable]
    public class StepMessage
    {
        [XmlElement(ElementName = "Text", IsNullable = true)]
        public string Text;

        [XmlElement(ElementName = "ItemID", IsNullable = true)]
        public int? ItemID;

        [XmlElement(ElementName = "Scale", IsNullable = true)]
        public float? Scale;
    }
}