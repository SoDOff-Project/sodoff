using System.ComponentModel.DataAnnotations;

namespace sodoff.Model
{
    public class HouseData
    {
        [Key]
        public int Id { get; set; }
        public int VikingId { get; set; }
        public virtual Viking Viking { get; set; } = null!;
        public string XmlData { get; set; } = null!;
    }
}
