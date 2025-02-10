using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepStoreItem", Namespace = "")]
    [Serializable]
    public class StepStoreItem
    {
        [XmlElement(ElementName = "StoreID")]
        public int StoreID;

        [XmlElement(ElementName = "ItemID")]
        public int ItemID;
    }
}