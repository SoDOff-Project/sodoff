using System.Xml.Serialization;

namespace sodoff.Schema;


[XmlRoot(ElementName = "BP", Namespace = "", IsNullable = true)]
[Serializable]
public class BluePrint {
    public BluePrint() {}

    public BluePrint(BluePrint other) {
        Deductibles = other.Deductibles.Select(d => new BluePrintDeductibleConfig(d)).ToList();
        Ingredients = other.Ingredients.Select(i => new BluePrintSpecification(i)).ToList();
        Outputs = other.Outputs.Select(o => new BluePrintSpecification(o)).ToList();
    }

    [XmlElement(ElementName = "BPDC", IsNullable = true)]
	public List<BluePrintDeductibleConfig> Deductibles { get; set; }

	[XmlElement(ElementName = "ING", IsNullable = false)]
	public List<BluePrintSpecification> Ingredients { get; set; }

	[XmlElement(ElementName = "OUT", IsNullable = false)]
	public List<BluePrintSpecification> Outputs { get; set; }
}
