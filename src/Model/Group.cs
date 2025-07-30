using sodoff.Schema;
using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class Group {
    [Key]
    public int Id { get; set; }

    public Guid GroupID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public GroupType Type { get; set; }

    public string? Logo { get; set; }

    public string? Color { get; set; }

    public int Points { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? LastActiveTime { get; set; }

    public uint GameID { get; set; }

    public int MaxMemberLimit { get; set; }

    public virtual ICollection<GroupViking> Vikings { get; set; } = null!;

    public virtual ICollection<GroupJoinRequest> JoinRequests { get; set; } = null!;
}
