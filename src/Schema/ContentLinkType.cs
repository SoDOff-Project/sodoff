using System.Xml.Serialization;

namespace sodoff.Schema
{
    public enum ContentLinkType
    {
        [XmlEnum("0")]
        Unknown,
        [XmlEnum("1")]
        OggSound,
        [XmlEnum("2")]
        OggMovie,
        [XmlEnum("3")]
        UnityWebPlayer,
        [XmlEnum("4")]
        UnityLevel
    }
}