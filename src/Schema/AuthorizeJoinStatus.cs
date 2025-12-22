using System.Xml.Serialization;

namespace sodoff.Schema;

public enum AuthorizeJoinStatus {
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
    ApproverNotInThisGroup,
    [XmlEnum("8")]
    ApproverHasNoPermission,
    [XmlEnum("9")]
    UserHasNoJoinRequest
}
