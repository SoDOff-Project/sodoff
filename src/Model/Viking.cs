using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using sodoff.Schema;

namespace sodoff.Model;

[Index(nameof(Uid))]
public class Viking {
    [Key]
    public int Id { get; set; }

    public Guid Uid { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public Guid UserId { get; set; }

    public string? AvatarSerialized { get; set; }

    public int? SelectedDragonId { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Dragon> Dragons { get; set; } = null!;
    public virtual ICollection<Image> Images { get; set; } = null!;
    public virtual ICollection<MissionState> MissionStates { get; set; } = null!;
    public virtual ICollection<TaskStatus> TaskStatuses { get; set; } = null!;
    public virtual ICollection<AchievementTaskState> AchievementTaskStates { get; set; } = null!;
    public virtual ICollection<Room> Rooms { get; set; } = null!;
    public virtual ICollection<SceneData> SceneData { get; set; } = null!;
    public virtual ICollection<AchievementPoints> AchievementPoints { get; set; } = null!;
    public virtual ICollection<PairData> PairData { get; set; } = null!;
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = null!;
    public virtual ICollection<GameData> GameData { get; set; } = null!;
    public virtual ICollection<ProfileAnswer> ProfileAnswers { get; set; } = null!;
    public virtual ICollection<SavedData> SavedData { get; set; } = null!;
    public virtual ICollection<Party> Parties { get; set; } = null!;
    public virtual ICollection<MMORole> MMORoles { get; set; } = null!;
    public virtual Neighborhood? Neighborhood { get; set; } = null!;
    public virtual ICollection<Group> Groups { get; set; } = null!;
    public virtual ICollection<Rating> Ratings { get; set; } = null!;
    public virtual Dragon? SelectedDragon { get; set; }

    public DateTime? CreationDate { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public uint? GameVersion { get; set; }
}
