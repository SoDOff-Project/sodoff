using System.Text.Json.Serialization;

namespace sodoff.Model;
public class SavedData {
    [JsonIgnore]
    public int VikingId { get; set; }
    public uint SaveId { get; set; }
    public string? SerializedData {  get; set; }

    [JsonIgnore]
    public virtual Viking Viking { get; set; } = null!;
}
