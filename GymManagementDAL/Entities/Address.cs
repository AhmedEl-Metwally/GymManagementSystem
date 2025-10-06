using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Entities
{
    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
