using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "StepsMissionsGroup", Namespace = "")]
[Serializable]
public class StepsMissionsGroup {
    [XmlElement(ElementName = "GameId")]
    public int GameId;

    [XmlElement(ElementName = "WorldName")]
    public string WorldName;

    [XmlElement(ElementName = "MissionData")]
    public MissionData MissionData;
}
