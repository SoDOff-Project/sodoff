using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GetGroupsRequest", IsNullable = true)]
[Serializable]
public class GetGroupsRequest {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "ForUserID")]
    public string ForUserID;

    [XmlElement(ElementName = "GroupID")]
    public Guid? GroupID;

    [XmlElement(ElementName = "Name")]
    public string Name;

    [XmlElement(ElementName = "ProductID")]
    public int? ProductID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID;

    [XmlElement(ElementName = "IncludeMemberCount")]
    public bool IncludeMemberCount;

    [XmlElement(ElementName = "IncludeMinFields")]
    public bool IncludeMinFields;

    [XmlElement(ElementName = "PageNo")]
    public int? PageNo;

    [XmlElement(ElementName = "PageSize")]
    public int? PageSize;

    [XmlElement(ElementName = "GroupsFilter")]
    public GroupsFilter GroupsFilter;
}
