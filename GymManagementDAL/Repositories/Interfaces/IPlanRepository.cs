using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Plan? GetPlanById(int id);
        IEnumerable<Plan> GetAllPlans();
        int updatePlan(Plan plan);
    }
}
