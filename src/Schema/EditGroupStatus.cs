using System.Xml.Serialization;

namespace sodoff.Schema;

public enum EditGroupStatus {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    Error,
    [XmlEnum("3")]
    UnableToGetUserDetails,
    [XmlEnum("4")]
    GroupNameInvalid,
    [XmlEnum("5")]
    GroupDescriptionInvalid,
    [XmlEnum("6")]
    GroupNameIsDuplicate,
    [XmlEnum("7")]
    GroupTypeIsInvalid,
    [XmlEnum("8")]
    PermissionDenied,
    [XmlEnum("9")]
    GroupNotFound,
    [XmlEnum("10")]
    GroupMaxMemberLimitInvalid,
    [XmlEnum("11")]
    GroupOwnerHasInSufficientCurrency
}
