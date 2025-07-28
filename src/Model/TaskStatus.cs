using System.Text.Json.Serialization;

namespace sodoff.Model {
    public class TaskStatus {
        public int Id { get; set; }

        public int MissionId { get; set; }

        [JsonIgnore]
        public int VikingId { get; set; }

        [JsonIgnore]
        public virtual Viking? Viking { get; set; }

        public string? Payload { get; set; }

        public bool Completed { get; set; } = false;
    }
}
