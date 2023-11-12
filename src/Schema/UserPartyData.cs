using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "Party", Namespace = "")]
    [Serializable]
    public class UserPartyData
    {
        [XmlElement(ElementName = "BuddyParties")]
        public UserParty[] BuddyParties;

        [XmlElement(ElementName = "NonBuddyParties")]
        public UserParty[] NonBuddyParties;
    }
}
