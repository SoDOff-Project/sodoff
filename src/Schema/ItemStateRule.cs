using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateRule", Namespace = "")]
[Serializable]
public class ItemStateRule {
    public ItemStateRule() { }

    public ItemStateRule(ItemStateRule other) {
        Criterias = other.Criterias.Select(c => new ItemStateCriteria(c)).ToList();
        CompletionAction = new CompletionAction(other.CompletionAction);
    }

    [XmlElement(ElementName = "Criterias")]
    public List<ItemStateCriteria> Criterias;

    [XmlElement(ElementName = "CompletionAction")]
    public CompletionAction CompletionAction;
}
