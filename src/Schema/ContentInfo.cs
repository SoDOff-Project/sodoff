using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "ArrayOfContentInfo", Namespace = "http://api.jumpstart.com/")]
    [Serializable]
    public class ContentInfo
    {
        [XmlElement("ContentInfo")]
        public ContentInfoData[] ContentInfoArray;
    }
}