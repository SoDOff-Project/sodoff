using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class DailyHighscore {
    [Key]
    public int Id { get; set; }
    public int VikingId { get; set; }
    public int GameId { get; set; }
    public int Difficulty { get; set; }
    public int GameLevel { get; set; }
    public bool IsMultiplayer { get; set; }
    public virtual ICollection<DailyHighscorePair> ScorePairs { get; set; } = null!;
    public virtual Viking Viking { get; set; } = null!;
}
