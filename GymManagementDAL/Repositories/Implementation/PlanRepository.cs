using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Implementation
{
    public class PlanRepository(GymDbContext _context) : IPlanRepository
    {
        public IEnumerable<Plan> GetAllPlans() => _context.Plans.ToList();      

        public Plan? GetPlanById(int id) => _context.Plans.Find(id);

        public int updatePlan(Plan plan)
        {
            _context.Plans.Update(plan);
            return _context.SaveChanges();  
        }
    }
}
