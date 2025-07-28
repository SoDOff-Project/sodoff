using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;

public class Rating {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int VikingId { get; set; }
    public int RankId { get; set; }

    public int Value { get; set; }
    public DateTime Date { get; set; }

    [JsonIgnore]
    public virtual Viking Viking { get; set; }
    public virtual RatingRank Rank { get; set; }
}
