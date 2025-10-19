using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class GroupJoinRequest {
    [Key]
    public int VikingID { get; set; }
    [Key]
    public int GroupID { get; set; }

    public string? Message { get; set; }

    public virtual Viking Viking { get; set; } = null!;
    public virtual Group Group { get; set; } = null!;
}
