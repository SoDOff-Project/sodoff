using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class GameDataPair {
    [Key]
    public int Id { get; set; }
    public int GameDataId { get; set; }
    public string Name { get; set; } = null!;
    public int Value { get; set; }
    public virtual GameData GameData { get; set; } = null!;
}
