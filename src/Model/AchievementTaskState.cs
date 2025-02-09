using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace sodoff.Model;

[PrimaryKey(nameof(TaskId), nameof(VikingId))]
public class AchievementTaskState {
    public int VikingId { get; set; }

    public int TaskId { get; set; }

    public int Points { get; set; }

    public virtual Viking? Viking { get; set; }
}
