using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Pairs", Namespace = "", IsNullable = true)]
[Serializable]
public class PairData {
    public PairData() {}

    public PairData(PairData other) {
        Pairs = other.Pairs.Select(p => new Pair(p)).ToArray();
    }

    [XmlElement("Pair", IsNullable = true)]
    public Pair[] Pairs { get; set; }
}