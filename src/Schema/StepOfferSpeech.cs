using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepOfferSpeech", Namespace = "")]
    [Serializable]
    public class StepOfferSpeech
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}