
namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        //JoinDate == CreatedAt of BaseEntity
        public string Photo { get; set; } = string.Empty;
    }
}
