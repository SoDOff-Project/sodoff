using System.ComponentModel.DataAnnotations;

namespace sodoff.Model
{
    public class ProfileAnswer
    {
        [Key]
        public int Id { get; set; }
        public int VikingId { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }

        public virtual Viking Viking { get; set; } = null!;
    }
}
