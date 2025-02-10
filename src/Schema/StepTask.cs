using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTask", Namespace = "")]
    [Serializable]
    public class StepTask
    {
        [XmlElement(ElementName = "TaskID")]
        public int TaskID;

        [XmlElement(ElementName = "Type")]
        public string Type;

        [XmlElement(ElementName = "Dialog", IsNullable = true)]
        public StepTaskDialog Dialog;

        [XmlElement(ElementName = "Message")]
        public StepTaskMessage[] Message;

        [XmlElement(ElementName = "SetupGroup", IsNullable = true)]
        public string SetupGroup;

        [XmlElement(ElementName = "SetupScene", IsNullable = true)]
        public string SetupScene;

        [XmlElement(ElementName = "Help")]
        public StepTaskHelp[] Help;

        [XmlElement(ElementName = "RewardPlayerItem")]
        public StepTaskRewardPlayerItem[] RewardPlayerItem;

        [XmlElement(ElementName = "Experience")]
        public int Experience;

        [XmlElement(ElementName = "Time", IsNullable = true)]
        public int? Time;

        [XmlElement(ElementName = "Objective")]
        public StepTaskObjective Objective;
    }
}
