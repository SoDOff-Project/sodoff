using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;

public class Room {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public string RoomId { get; set; }

    [JsonIgnore]
    public int VikingId { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual Viking? Viking { get; set; }

    public virtual ICollection<RoomItem> Items { get; set; } = null!;
}
