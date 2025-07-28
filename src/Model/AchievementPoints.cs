using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;
public class AchievementPoints {
    [JsonIgnore]
    public int VikingId { get; set; }

    public int Type { get; set; }

    public int Value { get; set; }

    [JsonIgnore]
    public virtual Viking? Viking { get; set; }
}
