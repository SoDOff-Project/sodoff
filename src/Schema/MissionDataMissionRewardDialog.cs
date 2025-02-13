using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "MissionDataMissionRewardDialog", Namespace = "")]
    [Serializable]
    public class MissionDataMissionRewardDialog
    {
        [XmlElement(ElementName = "FileName")]
        public string FileName;

        [XmlElement(ElementName = "NPC")]
        public string NPC;

        [XmlElement(ElementName = "Bundle")]
        public string Bundle;
    }
}