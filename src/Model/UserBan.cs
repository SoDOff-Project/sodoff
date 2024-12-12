using System;
using System.ComponentModel.DataAnnotations;
using sodoff.Schema;

namespace sodoff.Model;

public class UserBan
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid? UserId { get; set; } = null!;

    public UserBanType BanType { get; set; } = UserBanType.TemporarySuspension;
    public DateTime? CreatedAt { get; set; } = new DateTime();
    public DateTime? EndsAt { get; set; } = new DateTime();

    public virtual User User { get; set; } = null!;
}
