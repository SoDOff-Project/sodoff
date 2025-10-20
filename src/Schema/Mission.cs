using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Mission", Namespace = "")]
[Serializable]
public class Mission {
    public Mission() { }

    public Mission(Mission other) {
        if (other == null) throw new ArgumentNullException(nameof(other));
        MissionID = other.MissionID;
        Name = other.Name;
        GroupID = other.GroupID;
        ParentID = other.ParentID;
        Static = other.Static;
        Accepted = other.Accepted;
        Completed = other.Completed;
        Rule = other.Rule;
        VersionID = other.VersionID;
        AchievementID = other.AchievementID;
        AcceptanceAchievementID = other.AcceptanceAchievementID;
        Repeatable = other.Repeatable;
        MissionRule = new MissionRule(other.MissionRule);

        Missions = other.Missions.Select(m => new Mission(m)).ToList();
        Tasks = other.Tasks.Select(t => new Task(t)).ToList();
        Rewards = other.Rewards.Select(r => new AchievementReward(r)).ToList();
        AcceptanceRewards = other.AcceptanceRewards.Select(a => new AchievementReward(a)).ToList();
    }

    [XmlElement(ElementName = "I")]
    public int MissionID;

    [XmlElement(ElementName = "N")]
    public string Name;

    [XmlElement(ElementName = "G")]
    public int GroupID;

    [XmlElement(ElementName = "P", IsNullable = true)]
    public int? ParentID;

    [XmlElement(ElementName = "S")]
    public string Static;

    [XmlElement(ElementName = "A")]
    public bool Accepted;

    [XmlElement(ElementName = "C")]
    public int Completed;

    [XmlElement(ElementName = "R")]
    public string Rule;

    [XmlElement(ElementName = "MR")]
    public MissionRule MissionRule;

    [XmlElement(ElementName = "V")]
    public int VersionID;

    [XmlElement(ElementName = "AID")]
    public int AchievementID;

    [XmlElement(ElementName = "AAID")]
    public int AcceptanceAchievementID;

    [XmlElement(ElementName = "M")]
    public List<Mission> Missions;

    [XmlElement(ElementName = "Task")]
    public List<Task> Tasks;

    [XmlElement(ElementName = "AR")]
    public List<AchievementReward> Rewards;

    [XmlElement(ElementName = "AAR")]
    public List<AchievementReward> AcceptanceRewards;

    [XmlElement(ElementName = "RPT")]
    public bool Repeatable;
}
