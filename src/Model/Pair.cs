using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sodoff.Model;

public class Pair {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    [JsonIgnore]
    public int MasterId { get; set; }

    [JsonIgnore]
    public virtual PairData PairData { get; set; } = null!;
}
