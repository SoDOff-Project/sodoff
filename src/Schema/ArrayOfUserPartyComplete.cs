using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "ArrayOfUserPartyComplete", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfUserPartyComplete
{
    [XmlElement(ElementName = "UserPartyComplete")]
    public UserPartyComplete[] UserPartyComplete;
}
