using System.Xml.Serialization;
namespace sodoff.Schema;

[Serializable]
public class AuthenticationInfo {
    [XmlElement]
    public bool Authenticated { get; set; }

    [XmlElement]
    public string DisplayName { get; set; } = string.Empty;

    [XmlElement]
    public Role Role { get; set; } = Role.User;
}

[Serializable]
public enum Role {
    User = 0, Moderator = 1, Admin = 2
}
