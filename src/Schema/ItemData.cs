using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "I", Namespace = "", IsNullable = true)]
public class ItemData {
    public ItemData() {}

    public ItemData(ItemData other) {
        ItemStates = other.ItemStates.Select(s => new ItemState(s)).ToList();
        ItemRarity = other.ItemRarity;
        PossibleStatsMap = new ItemPossibleStatsMap(other.PossibleStatsMap);
        ItemStatsMap = new ItemStatsMap(other.ItemStatsMap);
        ItemSaleConfigs = other.ItemSaleConfigs.Select(cfg => new ItemSaleConfig(cfg)).ToArray();
        BluePrint = new BluePrint(other.BluePrint);
        AssetName = other.AssetName;
        Attribute = other.Attribute.Select(attr => new ItemAttribute(attr)).ToArray();
        Category = other.Category.Select(cat => new ItemDataCategory(cat)).ToArray();
        Cost = other.Cost;
        CashCost = other.CashCost;
        CreativePoints = other.CreativePoints;
        Description = other.Description;
        IconName = other.IconName;
        InventoryMax = other.InventoryMax;
        ItemID = other.ItemID;
        ItemName = other.ItemName;
        ItemNamePlural = other.ItemNamePlural;
        Locked = other.Locked;
        Geometry2 = other.Geometry2;
        Rollover = new ItemDataRollover(other.Rollover);
        RankId = other.RankId;
        Relationship = other.Relationship.Select(r => new ItemDataRelationship(r)).ToArray();
        Stackable = other.Stackable;
        AllowStacking = other.AllowStacking;
        SaleFactor = other.SaleFactor;
        Texture = other.Texture.Select(t => new ItemDataTexture(t)).ToArray();
        Uses = other.Uses;
        Availability = other.Availability.Select(a => new ItemAvailability(a)).ToArray();
        RewardTypeID = other.RewardTypeID;
        Points = other.Points;
        NormalDiscoutModifier = other.NormalDiscoutModifier;
        MemberDiscountModifier = other.MemberDiscountModifier;
    }

    [XmlElement(ElementName = "is")]
	public List<ItemState> ItemStates { get; set; }

	[XmlElement(ElementName = "ir", IsNullable = true)]
	public ItemRarity? ItemRarity { get; set; }

	[XmlElement(ElementName = "ipsm", IsNullable = true)]
	public ItemPossibleStatsMap PossibleStatsMap { get; set; }

	[XmlElement(ElementName = "ism", IsNullable = true)]
	public ItemStatsMap ItemStatsMap { get; set; }

	[XmlElement(ElementName = "iscs", IsNullable = true)]
	public ItemSaleConfig[] ItemSaleConfigs { get; set; }

	[XmlElement(ElementName = "bp", IsNullable = true)]
	public BluePrint BluePrint { get; set; }

	[XmlElement(ElementName = "an")]
	public string AssetName;

	[XmlElement(ElementName = "at", IsNullable = true)]
	public ItemAttribute[] Attribute;

	[XmlElement(ElementName = "c")]
	public ItemDataCategory[] Category;

	[XmlElement(ElementName = "ct")]
	public int Cost;

	[XmlElement(ElementName = "ct2")]
	public int CashCost;

	[XmlElement(ElementName = "cp")]
	public int CreativePoints;

	[XmlElement(ElementName = "d")]
	public string Description;

	[XmlElement(ElementName = "icn")]
	public string IconName;

	[XmlElement(ElementName = "im")]
	public int InventoryMax;

	[XmlElement(ElementName = "id")]
	public int ItemID;

	[XmlElement(ElementName = "itn")]
	public string ItemName;

	[XmlElement(ElementName = "itnp")]
	public string ItemNamePlural;

	[XmlElement(ElementName = "l")]
	public bool Locked;

	[XmlElement(ElementName = "g", IsNullable = true)]
	public string Geometry2;

	[XmlElement(ElementName = "ro", IsNullable = true)]
	public ItemDataRollover Rollover;

	[XmlElement(ElementName = "rid", IsNullable = true)]
	public int? RankId;

	[XmlElement(ElementName = "r")]
	public ItemDataRelationship[] Relationship;

	[XmlElement(ElementName = "s")]
	public bool Stackable;

	[XmlElement(ElementName = "as")]
	public bool AllowStacking;

	[XmlElement(ElementName = "sf")]
	public int SaleFactor;

	[XmlElement(ElementName = "t")]
	public ItemDataTexture[] Texture;

	[XmlElement(ElementName = "u")]
	public int Uses;

	[XmlElement(ElementName = "av")]
	public ItemAvailability[] Availability;

	[XmlElement(ElementName = "rtid")]
	public int RewardTypeID;

	[XmlElement(ElementName = "p", IsNullable = true)]
	public int? Points;

	[XmlIgnore]
	public float NormalDiscoutModifier;

	[XmlIgnore]
	public float MemberDiscountModifier;

	[XmlIgnore]
	public float FinalDiscoutModifier {
		get {
			return Math.Min(1f, (1f - NormalDiscoutModifier) * (1f - MemberDiscountModifier));
        }
	}
}
