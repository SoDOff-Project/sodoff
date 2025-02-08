using System.Xml.Serialization;

namespace sodoff.Schema;

public enum AchievementPointTypes {
    [XmlEnum("1")]
    PlayerXP = 1,
    
    [XmlEnum("2")]
    GameCurrency = 2, // gold
    
    [XmlEnum("3")]
    Unknown3 = 3, // unknown, used in MB AchievementTasks
    
    [XmlEnum("4")]
    Unknown4 = 4,
    
    [XmlEnum("5")]
    CashCurrency = 5, // gems
    
    [XmlEnum("6")]
    ItemReward = 6,
    
    [XmlEnum("8")]
    DragonXP = 8,
    
    [XmlEnum("9")]
    PlayerFarmingXP = 9,
    
    [XmlEnum("10")]
    PlayerFishingXP = 10,
    
    [XmlEnum("11")]
    Trophies = 11,
    
    [XmlEnum("12")]
    UDTPoints = 12,
    
    [XmlEnum("13")]
    Unknown13 = 13,
    
    [XmlEnum("21")]
    Unknown21 = 21,
}
