using GymManagementDAL.Entities.Enums;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        // HireDate --> CreatedAt of BaseEntity
        public Specialties Specialties  { get; set; }
    }
}
