using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class RatingRank {
    [Key]
    public int Id { get; set; }

    public int CategoryID { get; set; }
    public int? RatedEntityID { get; set; }
    public string? RatedUserID { get; set; }

    public int Rank { get; set; }
    public float RatingAverage { get; set; } // On a scale of 1-5
    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = null!;
}
