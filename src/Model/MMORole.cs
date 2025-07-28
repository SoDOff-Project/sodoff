using sodoff.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;

public class MMORole {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public int VikingId { get; set; }

    public Role Role { get; set; }

    [JsonIgnore]
    public virtual Viking? Viking { get; set; }
}
