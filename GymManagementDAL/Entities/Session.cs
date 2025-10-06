
namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
