using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model
{
    public class Party
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; } = null!;
        public int VikingId { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
        public bool? PrivateParty { get; set; }
        public string IconAsset { get; set; } = null!;
        public string LocationIconAsset { get; set; } = null!;
        public string AssetBundle { get; set; } = null!;
        public virtual Viking? Viking { get; set; }
        public uint GameVersion { get; set; } = 0!;
        public string PartyType { get; set; } = null!;
    }
}
