using System.Xml.Serialization;

namespace sodoff.Schema;

public enum RemoveMemberStatus {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    Error,
    [XmlEnum("3")]
    UserNotAMemberOfTheGroup,
    [XmlEnum("4")]
    UserHasNoPermission,
    [XmlEnum("5")]
    InvalidParameters
}
