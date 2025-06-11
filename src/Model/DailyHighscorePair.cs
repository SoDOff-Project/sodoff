using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class DailyHighscorePair {
    [Key]
    public int Id { get; set; }
    public int DailyScoreId { get; set; }
    public string Name { get; set; } = null!;
    public int Value { get; set; }
    public DateTime DatePlayed { get; set; }
    public virtual DailyHighscore DailyScore { get; set; } = null!;
}
