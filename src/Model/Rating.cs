using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class Rating {
    [Key]
    public int Id { get; set; }

    /// <summary>Viking that controls this data.</summary>
    public virtual Viking? Viking { get; set; }

    public virtual RatingRank? Rank { get; set; }

    public int VikingId { get; set; }

    public int RankId { get; set; } // Done this to prevent it from generating an unnecessary pairs table.

    public int CategoryID { get; set; }

    public int? RatedEntityID { get; set; }
    public string? RatedUserID { get; set; }

    public int Value { get; set; }

    public DateTime Date { get; set; }
}
