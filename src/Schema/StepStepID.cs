using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepStepID", Namespace = "")]
    [Serializable]
    public class StepStepID
    {
        [XmlText]
        public int Value;
    }
}