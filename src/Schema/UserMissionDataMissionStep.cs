using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "UserMissionDataMissionStep", Namespace = "")]
    [Serializable]
    public class UserMissionDataMissionStep
    {
        [XmlElement(ElementName = "StepId")]
        public int StepId;

        [XmlElement(ElementName = "TaskId")]
        public int[] TaskId;
    }
}