using sodoff.Schema;
using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class GroupMember {
    [Key]
    public int VikingID { get; set; }
    [Key]
    public int GroupID { get; set; }
    public UserRole UserRole { get; set; }
    public DateTime JoinDate { get; set; }

    public virtual Viking Viking { get; set; } = null!;
    public virtual Group Group { get; set; } = null!;
}
