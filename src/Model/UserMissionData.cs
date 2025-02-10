using System.ComponentModel.DataAnnotations;

namespace sodoff.Model
{
    public class UserMissionData
    {
        [Key]
        public int Id { get; set; }
        public int VikingId { get; set; }
        public int WorldId { get; set; }
        public int MissionId { get; set; }
        public int StepId { get; set; }
        public int TaskId { get; set; }
        public bool IsCompleted { get; set; } = false;

        public virtual Viking? Viking { get; set; }
    }
}
