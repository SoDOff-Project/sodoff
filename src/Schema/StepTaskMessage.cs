using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskMessage", Namespace = "")]
    [Serializable]
    public class StepTaskMessage
    {
        [XmlElement(ElementName = "Text", IsNullable = true)]
        public string Text;

        [XmlElement(ElementName = "ItemID", IsNullable = true)]
        public int? ItemID;

        [XmlElement(ElementName = "Scale", IsNullable = true)]
        public float? Scale;
    }
}