using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace sodoff.Model;

[PrimaryKey(nameof(TaskId), nameof(VikingId))]
public class AchievementTaskState {
    [JsonIgnore]
    public int VikingId { get; set; }

    public int TaskId { get; set; }

    public int Points { get; set; }

    [JsonIgnore]
    public virtual Viking? Viking { get; set; }
}
