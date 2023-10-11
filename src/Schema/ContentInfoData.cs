using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "ContentInfo")]
    [Serializable]
    public class ContentInfoData
    {
        [XmlElement(ElementName = "DisplayName")]
        public string? DisplayName;

        [XmlElement(ElementName = "Description")]
        public string? Description;

        [XmlElement(ElementName = "ThumbnailUrl")]
        public string? ThumbnailUrl;

        [XmlElement(ElementName = "LinkUrl")]
        public string? LinkUrl;

        [XmlElement(ElementName = "TextureUrl")]
        public string? TextureUrl;

        [XmlElement(ElementName = "RolloverUrl")]
        public string? RolloverUrl;

        [XmlElement(ElementName = "CategoryUrl")]
        public string? CategoryUrl;

        [XmlElement(ElementName = "ContentType")]
        public int? ContentType;

        [XmlElement(ElementName = "MembersOnly")]
        public bool? MembersOnly;

        [XmlElement(ElementName = "LinkType")]
        public ContentLinkType? LinkType;
    }
}