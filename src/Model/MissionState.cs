using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model;

public class MissionState {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public int MissionId { get; set; }

    [JsonIgnore]
    public int VikingId { get; set; }

    [JsonIgnore]
    public virtual Viking? Viking { get; set; }

    public MissionStatus MissionStatus { get; set; }

    public bool? UserAccepted { get; set; }
}

public enum MissionStatus {
    Upcoming,Active,Completed
}
