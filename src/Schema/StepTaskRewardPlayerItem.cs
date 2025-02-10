using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskRewardPlayerItem", Namespace = "")]
    [Serializable]
    public class StepTaskRewardPlayerItem
    {
        [XmlElement(ElementName = "ItemID")]
        public int ItemID;

        [XmlElement(ElementName = "Quantity")]
        public int Quantity;
    }
}