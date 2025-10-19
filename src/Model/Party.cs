using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model
{
    public class Party
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Location { get; set; } = null!;
        [JsonIgnore]
        public int VikingId { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
        public bool? PrivateParty { get; set; }
        public string LocationIconAsset { get; set; } = null!;
        public string AssetBundle { get; set; } = null!;
        [JsonIgnore]
        public virtual Viking? Viking { get; set; }
    }
}
