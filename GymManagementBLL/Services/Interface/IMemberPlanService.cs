using GymManagementBLL.ViewModels.MemberPlanViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberPlanService
    {
        IEnumerable<MemberPlanViewModel> GetAllMemberPlans();
    }
}
