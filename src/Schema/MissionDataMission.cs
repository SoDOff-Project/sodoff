using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "MissionDataMission", Namespace = "")]
    [Serializable]
    public class MissionDataMission
    {
        [XmlElement(ElementName = "MissionID")]
        public int MissionID;

        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "DisplayName", IsNullable = true)]
        public string DisplayName;

        [XmlElement(ElementName = "IconName", IsNullable = true)]
        public string IconName;

        [XmlElement(ElementName = "Description", IsNullable = true)]
        public string Description;

        [XmlElement(ElementName = "Experience")]
        public int Experience;

        [XmlElement(ElementName = "RewardDialog", IsNullable = true)]
        public MissionDataMissionRewardDialog RewardDialog;

        [XmlElement(ElementName = "UnlockMission")]
        public int[] UnlockMission;

        [XmlElement(ElementName = "Step", IsNullable = true)]
        public MissionDataMissionStep[] Step;
    }
}