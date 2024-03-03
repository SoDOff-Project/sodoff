namespace sodoff.Model;
public class SavedData {
    public int VikingId { get; set; }
    public uint SaveId { get; set; }
    public string? SerializedData {  get; set; }

    public virtual Viking Viking { get; set; } = null!;
}
