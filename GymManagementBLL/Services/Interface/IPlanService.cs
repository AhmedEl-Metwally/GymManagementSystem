
using GymManagementBLL.ViewModels.PlanViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int PlanId);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan);
        bool ToggleStatus(int planId);
    }
}
