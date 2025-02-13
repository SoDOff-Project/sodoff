using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskObjective", Namespace = "")]
    [Serializable]
    public class StepTaskObjective
    {
        [XmlElement(ElementName = "Beacon", IsNullable = true)]
        public bool? Beacon;

        [XmlElement(ElementName = "NPC", IsNullable = true)]
        public string NPC;

        [XmlElement(ElementName = "Marker", IsNullable = true)]
        public string Marker;

        [XmlElement(ElementName = "Scene", IsNullable = true)]
        public string Scene;

        [XmlElement(ElementName = "Range", IsNullable = true)]
        public float? Range;

        [XmlElement(ElementName = "Module", IsNullable = true)]
        public string Module;

        [XmlElement(ElementName = "Group", IsNullable = true)]
        public string Group;

        [XmlElement(ElementName = "Object", IsNullable = true)]
        public string Object;

        [XmlElement(ElementName = "StoreID", IsNullable = true)]
        public int? StoreID;

        [XmlElement(ElementName = "ItemID", IsNullable = true)]
        public int? ItemID;

        [XmlElement(ElementName = "ItemName", IsNullable = true)]
        public string ItemName;

        [XmlElement(ElementName = "CategoryID", IsNullable = true)]
        public int? CategoryID;

        [XmlElement(ElementName = "AttributeID")]
        public int[] AttributeID;

        [XmlElement(ElementName = "Quantity", IsNullable = true)]
        public int? Quantity;

        [XmlElement(ElementName = "Photo", IsNullable = true)]
        public StepTaskObjectivePhoto Photo;

        [XmlElement(ElementName = "Creative", IsNullable = true)]
        public StepTaskObjectiveCreative Creative;
    }
}
