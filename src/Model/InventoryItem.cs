using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model {
    public class InventoryItem {
        [Key]
        // [JsonIgnore] used as room id, used in serialised xml (pairs, ...)
        public int Id {  get; set; }

        public int ItemId { get; set; }

        [JsonIgnore]
        public int VikingId { get; set; }

        public string? StatsSerialized { get; set; }

        public string? AttributesSerialized { get; set; }

        [JsonIgnore]
        public virtual Viking Viking { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
