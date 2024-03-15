using System.ComponentModel.DataAnnotations;

namespace sodoff.Model
{
    public class SceneData
    {
        [Key]
        public int Id { get; set; }
        public int VikingId { get; set; }
        public string SceneName { get; set; } = null!;
        public string XmlData {  get; set; } = null!;
        public virtual Viking Viking { get; set; } = null!;
    }
}
