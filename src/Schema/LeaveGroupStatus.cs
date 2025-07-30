using System.Xml.Serialization;

namespace sodoff.Schema;

public enum LeaveGroupStatus {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    Error,
    [XmlEnum("3")]
    UserNotAMemberOfTheGroup
}
