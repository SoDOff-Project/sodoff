using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model
{
    public class SceneData
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int VikingId { get; set; }
        public string SceneName { get; set; } = null!;
        public string XmlData {  get; set; } = null!;

        [JsonIgnore]
        public virtual Viking Viking { get; set; } = null!;
    }
}
