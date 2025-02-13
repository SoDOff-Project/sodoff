using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepNPCData", Namespace = "")]
    [Serializable]
    public class StepNPCData
    {
        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Marker")]
        public string Marker;

        [XmlElement(ElementName = "Scene")]
        public string Scene;

        [XmlElement(ElementName = "Animation", IsNullable = true)]
        public string Animation;
    }
}