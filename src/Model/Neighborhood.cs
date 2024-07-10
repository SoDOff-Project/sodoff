using sodoff.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace sodoff.Model
{
    public class Neighborhood
    {
        public virtual Viking? Viking { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
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
