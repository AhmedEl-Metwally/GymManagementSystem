
using GymManagementDAL.Entities.Enums;

namespace GymManagementDAL.Entities
{
    public abstract class GymUser : BaseEntity
    {
        public string Name  { get; set; } = string.Empty;
        public string Email  { get; set; } = string.Empty;
        public string Phone  { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender  { get; set; }
        public Address Address { get; set; } = new Address();
    }
}
