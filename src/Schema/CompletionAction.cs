using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class CompletionAction {
    public CompletionAction() { }

    public CompletionAction(CompletionAction other) {
        Transition = other.Transition;
    }

    [XmlElement(ElementName = "Transition")]
    public StateTransition Transition;

    [XmlElement(ElementName = "AchievementCompletion", IsNullable = true)]
    public AchievementCompletion[] AchievementCompletion;
}
