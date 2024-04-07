using sodoff.Schema;
using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class MMORole {
    [Key]
    public int Id { get; set; }

    public int VikingId { get; set; }

    public Role Role { get; set; }

    public virtual Viking? Viking { get; set; }
}
