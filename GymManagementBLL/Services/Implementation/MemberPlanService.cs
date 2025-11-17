using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberPlanViewModels;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class MemberPlanService(IUnitOfWork _unitOfWork,IMapper _mapper) : IMemberPlanService
    {
        public IEnumerable<MemberPlanViewModel> GetAllMemberPlans()
        {
            var memberPlan = _unitOfWork.MemberPlanRepository.GetAllMemberPlanWithMembersAndPlans(M => M.Status =="Active");
            var memberPlanViewModel = _mapper.Map<IEnumerable<MemberPlanViewModel>>(memberPlan);
            return memberPlanViewModel;
        }
    }
}
