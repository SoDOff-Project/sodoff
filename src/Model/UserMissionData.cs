using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace sodoff.Model
{
    [PrimaryKey(nameof(VikingId), nameof(WorldId), nameof(MissionId))]
    public class UserMissionData
    {
        public int VikingId { get; set; }
        public int WorldId { get; set; }
        public int MissionId { get; set; }
        public int StepId { get; set; }
        public int TaskId { get; set; }
        public bool IsCompleted { get; set; } = false;

        public virtual Viking? Viking { get; set; }
    }
}
