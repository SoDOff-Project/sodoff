using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MissionRule", Namespace = "")]
[Serializable]
public class MissionRule {
    public MissionRule() {}

    public MissionRule(MissionRule other) {
        Prerequisites = other.Prerequisites.Select(p => new PrerequisiteItem(p)).ToList();
        Criteria = new MissionCriteria(other.Criteria);
    }

    [XmlElement(ElementName = "Prerequisites")]
    public List<PrerequisiteItem> Prerequisites;

    [XmlElement(ElementName = "Criteria")]
    public MissionCriteria Criteria;
}
