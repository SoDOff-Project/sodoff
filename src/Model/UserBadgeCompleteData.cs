using System.Text.Json.Serialization;

namespace sodoff.Model
{
    public class UserBadgeCompleteData
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int VikingId { get; set; }
        public int BadgeId { get; set; }

        [JsonIgnore]
        public virtual Viking? Viking { get; set; }
    }
}
