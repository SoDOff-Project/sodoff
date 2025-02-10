using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "StepTaskObjective", Namespace = "")]
    [Serializable]
    public class StepTaskObjective
    {
        [XmlElement(ElementName = "Beacon", IsNullable = true)]
        public bool? Beacon;

        // Token: 0x0400045B RID: 1115
        [XmlElement(ElementName = "NPC", IsNullable = true)]
        public string NPC;

        // Token: 0x0400045C RID: 1116
        [XmlElement(ElementName = "Marker", IsNullable = true)]
        public string Marker;

        // Token: 0x0400045D RID: 1117
        [XmlElement(ElementName = "Scene", IsNullable = true)]
        public string Scene;

        // Token: 0x0400045E RID: 1118
        [XmlElement(ElementName = "Range", IsNullable = true)]
        public float? Range;

        // Token: 0x0400045F RID: 1119
        [XmlElement(ElementName = "Module", IsNullable = true)]
        public string Module;

        // Token: 0x04000460 RID: 1120
        [XmlElement(ElementName = "Group", IsNullable = true)]
        public string Group;

        // Token: 0x04000461 RID: 1121
        [XmlElement(ElementName = "Object", IsNullable = true)]
        public string Object;

        // Token: 0x04000462 RID: 1122
        [XmlElement(ElementName = "StoreID", IsNullable = true)]
        public int? StoreID;

        // Token: 0x04000463 RID: 1123
        [XmlElement(ElementName = "ItemID", IsNullable = true)]
        public int? ItemID;

        // Token: 0x04000464 RID: 1124
        [XmlElement(ElementName = "ItemName", IsNullable = true)]
        public string ItemName;

        // Token: 0x04000465 RID: 1125
        [XmlElement(ElementName = "CategoryID", IsNullable = true)]
        public int? CategoryID;

        // Token: 0x04000466 RID: 1126
        [XmlElement(ElementName = "AttributeID")]
        public int[] AttributeID;

        // Token: 0x04000467 RID: 1127
        [XmlElement(ElementName = "Quantity", IsNullable = true)]
        public int? Quantity;

        // Token: 0x04000468 RID: 1128
        [XmlElement(ElementName = "Photo", IsNullable = true)]
        public StepTaskObjectivePhoto Photo;

        // Token: 0x04000469 RID: 1129
        [XmlElement(ElementName = "Creative", IsNullable = true)]
        public StepTaskObjectiveCreative Creative;
    }
}