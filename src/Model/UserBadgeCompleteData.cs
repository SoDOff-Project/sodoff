namespace sodoff.Model
{
    public class UserBadgeCompleteData
    {
        public int Id { get; set; }
        public int VikingId { get; set; }
        public int BadgeId { get; set; }

        public virtual Viking? Viking { get; set; }
    }
}
