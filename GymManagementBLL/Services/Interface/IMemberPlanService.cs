using GymManagementBLL.ViewModels.MemberPlanViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberPlanService
    {
        IEnumerable<MemberPlanViewModel> GetAllMemberPlans();
        IEnumerable<MemberForSelectListViewModel> GetMemberForDropdown();
        IEnumerable<PlanForSelectListViewModel> GetPlanForDropdown();
        bool CreateMemberPlan(CreateMemberPlanViewModel createMemberPlanViewModel);
    }
}
