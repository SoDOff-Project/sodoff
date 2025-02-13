using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "UserMissionData", Namespace = "")]
    [Serializable]
    public class UserMissionData
    {
        [XmlElement(ElementName = "Mission")]
        public UserMissionDataMission[] Mission;

        [XmlElement(ElementName = "MissionComplete")]
        public int[] MissionComplete;
    }
}
