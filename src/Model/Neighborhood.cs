using sodoff.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model
{
    public class Neighborhood
    {
        [JsonIgnore]
        public virtual Viking? Viking { get; set; }

        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [JsonIgnore]
        public int VikingId { get; set; }

        [Required]
        public Guid Slot0 { get; set; }

        [Required]
        public Guid Slot1 { get; set; }

        [Required]
        public Guid Slot2 { get; set; }

        [Required]
        public Guid Slot3 { get; set; }

        [Required]
        public Guid Slot4 { get; set; }
    }
}
