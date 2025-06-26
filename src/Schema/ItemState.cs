using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemState", Namespace = "")]
[Serializable]
public class ItemState {
    public ItemState() { }

    public ItemState(ItemState other) {
        ItemStateID = other.ItemStateID;
        Name = other.Name;
        Rule = new ItemStateRule(other.Rule);
        Order = other.Order;
        AchievementID = other.AchievementID;
        Rewards = other.Rewards.Select(r => new AchievementReward(r)).ToArray();
    }

    [XmlElement(ElementName = "ItemStateID")]
	public int ItemStateID;

	[XmlElement(ElementName = "Name")]
	public string Name;

	[XmlElement(ElementName = "Rule")]
	public ItemStateRule Rule;

	[XmlElement(ElementName = "Order")]
	public int Order;

	[XmlElement(ElementName = "AchievementID", IsNullable = true)]
	public int? AchievementID;

	[XmlElement(ElementName = "Rewards")]
	public AchievementReward[] Rewards;
}
