using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTask", Namespace = "")]
    [Serializable]
    public class StepTask
    {
        [XmlElement(ElementName = "TaskID")]
        public int TaskID;

        // Token: 0x0400043C RID: 1084
        [XmlElement(ElementName = "Type")]
        public string Type;

        // Token: 0x0400043D RID: 1085
        [XmlElement(ElementName = "Dialog", IsNullable = true)]
        public StepTaskDialog Dialog;

        // Token: 0x0400043E RID: 1086
        [XmlElement(ElementName = "Message")]
        public StepTaskMessage[] Message;

        // Token: 0x0400043F RID: 1087
        [XmlElement(ElementName = "SetupGroup", IsNullable = true)]
        public string SetupGroup;

        // Token: 0x04000440 RID: 1088
        [XmlElement(ElementName = "SetupScene", IsNullable = true)]
        public string SetupScene;

        // Token: 0x04000441 RID: 1089
        [XmlElement(ElementName = "Help")]
        public StepTaskHelp[] Help;

        // Token: 0x04000442 RID: 1090
        [XmlElement(ElementName = "RewardPlayerItem")]
        public StepTaskRewardPlayerItem[] RewardPlayerItem;

        // Token: 0x04000443 RID: 1091
        [XmlElement(ElementName = "Experience")]
        public int Experience;

        // Token: 0x04000444 RID: 1092
        [XmlElement(ElementName = "Time", IsNullable = true)]
        public int? Time;

        // Token: 0x04000445 RID: 1093
        [XmlElement(ElementName = "Objective")]
        public StepTaskObjective Objective;
    }
}