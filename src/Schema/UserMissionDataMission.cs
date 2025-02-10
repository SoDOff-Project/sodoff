using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "UserMissionDataMission", Namespace = "")]
    [Serializable]
    public class UserMissionDataMission
    {
        [XmlElement(ElementName = "MissionId")]
        public int MissionId;

        [XmlElement(ElementName = "Step")]
        public UserMissionDataMissionStep[] Step;
    }
}