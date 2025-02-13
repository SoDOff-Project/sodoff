using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "MissionData", Namespace = "", IsNullable = false)]
    [Serializable]
    public class MissionData
    {
        [XmlElement(ElementName = "Mission")]
        public MissionDataMission[] Mission;
    }
}
