using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberPlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;
using System.Globalization;

namespace GymManagementBLL.Services.Implementation
{
    public class MemberPlanService(IUnitOfWork _unitOfWork,IMapper _mapper) : IMemberPlanService
    {
        public IEnumerable<MemberPlanViewModel> GetAllMemberPlans()
        {
            var memberPlan = _unitOfWork.MemberPlanRepository.GetAllMemberPlanWithMembersAndPlans(M => M.Status =="Active").ToList();
            var memberPlanViewModel = _mapper.Map<IEnumerable<MemberPlanViewModel>>(memberPlan);
            return memberPlanViewModel;
        }

        public bool CreateMemberPlan(CreateMemberPlanViewModel createMemberPlanViewModel)
        {
            if(!IsMemberExists(createMemberPlanViewModel.MemberId) || !IsPlanExists(createMemberPlanViewModel.PlanId) || HasActiveMemberShips(createMemberPlanViewModel.MemberId))
                return false;

            var plan = _unitOfWork.GetRepository<Plan>().GetById(createMemberPlanViewModel.PlanId);
            var memberPlan = _unitOfWork.GetRepository<MemberPlan>();
            var memberPlanToCreate = _mapper.Map<MemberPlan>(createMemberPlanViewModel);

            memberPlanToCreate.EndDate = DateTime.UtcNow.AddDays(plan!.DurationDays);
            memberPlan.Add(memberPlanToCreate);
            return _unitOfWork.SaveChange() > 0;
        }

        public IEnumerable<MemberForSelectListViewModel> GetMemberForDropdown()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            var MemberSelectList = _mapper.Map<IEnumerable<MemberForSelectListViewModel>>(members);
            return MemberSelectList;
        }

        public IEnumerable<PlanForSelectListViewModel> GetPlanForDropdown()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll(P => P.IsActive);
            var PlanSelectList = _mapper.Map<IEnumerable<PlanForSelectListViewModel>>(plans);
            return PlanSelectList;
        }

        //Helper Methods

        private bool IsMemberExists(int memberId) => _unitOfWork.GetRepository<Member>().GetById(memberId) != null;
        private bool IsPlanExists(int planId) => _unitOfWork.GetRepository<Plan>().GetById(planId) != null;
        private bool HasActiveMemberShips(int memberId)
            => _unitOfWork.MemberPlanRepository.GetAllMemberPlanWithMembersAndPlans(M => M.Status.ToLower() == "active" && M.MemberId == memberId).Any();

    }
}
