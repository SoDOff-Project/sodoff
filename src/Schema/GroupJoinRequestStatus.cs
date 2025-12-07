using System.Xml.Serialization;

namespace sodoff.Schema;

public enum GroupJoinRequestStatus {
    [XmlEnum("1")]
    Pending = 1,
    [XmlEnum("2")]
    Approved,
    [XmlEnum("3")]
    Rejected,
    [XmlEnum("4")]
    Cancelled,
    [XmlEnum("5")]
    PendingAccountOwnerRequest
}