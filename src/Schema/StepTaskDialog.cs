using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskDialog", Namespace = "")]
    [Serializable]
    public class StepTaskDialog
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}