using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepStartPlayerItem", Namespace = "")]
    [Serializable]
    public class StepStartPlayerItem
    {
        [XmlElement(ElementName = "ItemID")]
        public int ItemID;

        [XmlElement(ElementName = "Quantity")]
        public int Quantity;
    }
}