using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sodoff.Model
{
    public class ProfileAnswer
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int VikingId { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }

        [JsonIgnore]
        public virtual Viking Viking { get; set; } = null!;
    }
}
