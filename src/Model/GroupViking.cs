using sodoff.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model;

public class GroupViking {
    [Key]
    public int VikingID { get; set; }
    [Key]
    public int GroupID { get; set; }
    public UserRole UserRole { get; set; }
    public DateTime JoinDate { get; set; }

    public virtual Viking Viking { get; set; } = null!;
    public virtual Group Group { get; set; } = null!;

    [NotMapped]
    public string Name { get { return Group.Name; } }

    [NotMapped]
    public string? Description { get { return Group.Description; } }

    [NotMapped]
    public GroupType Type { get { return Group.Type; } }

    [NotMapped]
    public string Logo { get { return Group.Logo; } }

    [NotMapped]
    public string Color { get { return Group.Color; } }

    [NotMapped]
    public uint GameID { get { return Group.GameID; } }
}
