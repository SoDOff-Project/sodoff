using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "ArrayOfCombinedListMessage")]
    [Serializable]
    public class ArrayOfCombinedListMessage
    {
        [XmlElement("CombinedListMessage")]
        public CombinedListMessage[] CombinedListMessage;
    }
}