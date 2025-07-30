using System.Xml.Serialization;

namespace sodoff.Schema;

public enum AssignRoleStatus {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    Error,
    [XmlEnum("3")]
    ApproverNotMemberOfTheGroup,
    [XmlEnum("4")]
    ApproverHasNoPermission,
    [XmlEnum("5")]
    MemberAlreadyInTheRole,
    [XmlEnum("6")]
    MemberNotPartOfTheGroup
}
