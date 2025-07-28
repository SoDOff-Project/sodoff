using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;

[Index(nameof(Username), IsUnique = true)]
public class User {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Session> Sessions { get; set; } = null!;
    public virtual ICollection<Viking> Vikings { get; set; } = null!;
    public virtual ICollection<PairData> PairData { get; set; } = null!;
}
