using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "Party", Namespace = "")]
    [Serializable]
    public class UserParty
    {
        [XmlElement(ElementName = "UID")]
        public Guid UserID;

        [XmlElement(ElementName = "UserName")]
        public string UserName;

        [XmlElement(ElementName = "Name")]
        public string DisplayName;

        [XmlElement(ElementName = "Icon")]
        public string Icon;

        [XmlElement(ElementName = "Loc")]
        public string Location;

        [XmlElement(ElementName = "LocIcon")]
        public string LocationIcon;

        [XmlElement(ElementName = "ExpDate")]
        public DateTime ExpirationDate;

        [XmlElement(ElementName = "Pvt")]
        public bool PrivateParty;
    }
}
