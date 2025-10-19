using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sodoff.Model;

[Index(nameof(EntityId), IsUnique = true)]
public class Dragon {
    [Key]
    // [JsonIgnore] used in serialised xml (stables)
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid EntityId { get; set; }

    [Required]
    [JsonIgnore]
    public int VikingId { get; set; }

    public string? RaisedPetData { get; set; }

    public int? PetXP { get; set; }

    [JsonIgnore]
    public virtual Viking Viking { get; set; } = null!;
    public virtual ICollection<PairData> PairData { get; set; } = null!;
}
