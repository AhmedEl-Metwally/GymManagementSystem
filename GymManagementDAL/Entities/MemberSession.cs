
namespace GymManagementDAL.Entities
{
    public class MemberSession : BaseEntity
    {
        //BookingDate == CreatedAt of BaseEntity
        public bool IsAttended { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = default!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = default!;
    }
}
