using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepEndSpeech", Namespace = "")]
    [Serializable]
    public class StepEndSpeech
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}