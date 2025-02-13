using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "MissionDataMissionStep", Namespace = "")]
    [Serializable]
    public class MissionDataMissionStep
    {
        [XmlElement(ElementName = "StepID")]
        public int StepID;

        [XmlElement(ElementName = "TaskID")]
        public int[] TaskID;
    }
}