
namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        //JoinDate == CreatedAt of BaseEntity
        public string Photo { get; set; } = string.Empty;

        public HealthRecord HealthRecord  { get; set; } = default!;

        public ICollection<MemberPlan> MemberPlans { get; set; } = new List<MemberPlan>();

        public ICollection<MemberSession> MemberSessions { get; set; } = new List<MemberSession>();
    }
}
