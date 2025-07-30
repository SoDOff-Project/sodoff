using System.Xml.Serialization;

namespace sodoff.Schema;

public enum JoinGroupStatus {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    Error,
    [XmlEnum("3")]
    GroupIsFull,
    [XmlEnum("4")]
    GroupTypeIsNotPublic,
    [XmlEnum("5")]
    UserAlreadyMemberOfTheGroup,
    [XmlEnum("6")]
    GroupTypeIsPublic,
    [XmlEnum("7")]
    MessageInvalid,
    [XmlEnum("8")]
    JoinRequestPending,
    [XmlEnum("9")]
    InviteNeededToJoin,
    [XmlEnum("10")]
    NoValidInvite,
    [XmlEnum("11")]
    InviterHasNoPermission
}
