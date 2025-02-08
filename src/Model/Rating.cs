using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class Rating {
    [Key]
    public int Id { get; set; }

    public int VikingId { get; set; }
    public int RankId { get; set; }

    public int Value { get; set; }
    public DateTime Date { get; set; }

    public virtual Viking Viking { get; set; }
    public virtual RatingRank Rank { get; set; }
}
