
namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;

        public ICollection<MemberSession> MemberSessions { get; set; } = new List<MemberSession>();
    }
}
