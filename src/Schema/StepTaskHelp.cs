using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskHelp", Namespace = "")]
    [Serializable]
    public class StepTaskHelp
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}