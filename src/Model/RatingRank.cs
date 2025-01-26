using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace sodoff.Model;

public class RatingRank {
    [Key]
    public int Id { get; set; }

    public int CategoryID { get; set; }

    public int? RatedEntityID { get; set; }
    public string? RatedUserID { get; set; }

    public int Rank { get; set; }

    /// <summary>On a scale of 1-5</summary>
    public float RatingAverage { get; set; }

    public int TotalVotes { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = null!;
}
