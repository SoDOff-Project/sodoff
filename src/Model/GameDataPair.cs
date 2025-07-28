using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;
public class GameDataPair {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int GameDataId { get; set; }
    public string Name { get; set; } = null!;
    public int Value { get; set; }
    [JsonIgnore]
    public virtual GameData GameData { get; set; } = null!;
}
