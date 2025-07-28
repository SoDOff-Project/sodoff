using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sodoff.Model;

[Index(nameof(UserId))]
[Index(nameof(VikingId))]
public class PairData {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public int Id { get; set; }

    public int PairId { get; set; }

    [JsonIgnore]
    public Guid? UserId { get; set; }

    [JsonIgnore]
    public int? VikingId { get; set; }

    [JsonIgnore]
    public int? DragonId { get; set; }

    public virtual ICollection<Pair> Pairs { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual Viking? Viking { get; set; }

    [JsonIgnore]
    public virtual Dragon? Dragon { get; set; }
}
