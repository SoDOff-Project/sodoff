using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "EditGroupRequest", IsNullable = true)]
[Serializable]
public class EditGroupRequest {
    [XmlElement(ElementName = "GroupID")]
    public string GroupID;

    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "Name")]
    public string Name;

    [XmlElement(ElementName = "Description")]
    public string Description;

    [XmlElement(ElementName = "Type")]
    public GroupType? Type;

    [XmlElement(ElementName = "Logo")]
    public string Logo;

    [XmlElement(ElementName = "Color")]
    public string Color;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;

    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;
    
    [XmlElement(ElementName = "MaxMemberLimit")]
    public int? MaxMemberLimit;
}
