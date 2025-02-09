using sodoff.Schema;
using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

// Implementation for EMD, add whatever else if needed
public class Group {
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid GroupID { get; set; }

    public string Name { get; set; }

    public GroupType Type { get; set; }

    public string Color { get; set; }

    public string Logo { get; set; }

    public string ApiKey { get; set; }

    public virtual ICollection<Viking> Vikings { get; set; } = null!;
}
