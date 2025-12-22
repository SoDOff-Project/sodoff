using System.Xml.Serialization;

namespace sodoff.Schema;

public enum CreateGroupStatus {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    Error,
    [XmlEnum("3")]
    GroupNameInvalid,
    [XmlEnum("4")]
    GroupDescriptionInvalid,
    [XmlEnum("5")]
    GroupNameIsDuplicate,
    [XmlEnum("6")]
    GroupOwnerHasInSufficientCurrency,
    [XmlEnum("7")]
    UnableToGetCreatorDetails,
    [XmlEnum("8")]
    CreatorIsNotApproved,
    [XmlEnum("9")]
    GroupNameIsEmpty,
    [XmlEnum("10")]
    GroupDescriptionIsEmpty,
    [XmlEnum("11")]
    GroupTypeIsInvalid,
    [XmlEnum("12")]
    GroupMaxMemberLimitInvalid,
    [XmlEnum("13")]
    AutomaticItemPurchaseFailed
}
