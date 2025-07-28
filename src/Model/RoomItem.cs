using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;
public class RoomItem {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int RoomId { get; set; }

    [JsonIgnore]
    public virtual Room Room { get; set; }

    public string RoomItemData { get; set; }
}
