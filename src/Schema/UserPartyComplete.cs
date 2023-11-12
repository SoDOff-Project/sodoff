using sodoff.Schema;
using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "PartyComplete", Namespace = "")]
[Serializable]
public class UserPartyComplete : UserParty
{
    [XmlElement(ElementName = "Asset")]
    public string AssetBundle;
}
