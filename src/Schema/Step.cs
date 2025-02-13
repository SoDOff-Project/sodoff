using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "Step", Namespace = "", IsNullable = false)]
    [Serializable]
    public class Step
    {
        [XmlElement(ElementName = "StepID")]
        public StepStepID StepID;

        [XmlElement(ElementName = "OfferSpeech", IsNullable = true)]
        public StepOfferSpeech OfferSpeech;

        [XmlElement(ElementName = "EndSpeech", IsNullable = true)]
        public StepEndSpeech EndSpeech;

        [XmlElement(ElementName = "TasksNeeded")]
        public int TasksNeeded;

        [XmlElement(ElementName = "Task")]
        public StepTask[] Task;

        [XmlElement(ElementName = "Message")]
        public StepMessage[] Message;

        [XmlElement(ElementName = "NPCData")]
        public StepNPCData[] NPCData;

        [XmlElement(ElementName = "StoreItem")]
        public StepStoreItem[] StoreItem;

        [XmlElement(ElementName = "StartPlayerItem")]
        public StepStartPlayerItem[] StartPlayerItem;
    }
}
