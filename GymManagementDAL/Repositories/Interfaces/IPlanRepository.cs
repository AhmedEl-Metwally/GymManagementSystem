using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Plan? GetPlanById(int id);
        IEnumerable<Plan> GetAllPlans();
        int updatePlan(Plan plan);
    }
}
