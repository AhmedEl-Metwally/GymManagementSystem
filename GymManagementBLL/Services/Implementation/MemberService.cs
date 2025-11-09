using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class MemberService(IUnitOfWork _unitOfWork,IMapper _mapper) : IMemberService
    {
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Menbers = _unitOfWork.GetRepository<Member>().GetAll();
            if (Menbers is null || !Menbers.Any())
                return Enumerable.Empty<MemberViewModel>();

            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Menbers);

            return MemberViewModels;
        }


        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
                    return false;

                var member = _mapper.Map<CreateMemberViewModel,Member>(createMember);
                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChange() > 0;
            }
            catch (Exception) 
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if(Member is null)
                return null;

            var viewModel = _mapper.Map<MemberViewModel>(Member);

            var ActiveMemberPlan = _unitOfWork.GetRepository<MemberPlan>().GetAll(M =>M.MemberId == MemberId && M.Status == "Active").FirstOrDefault();
            if (ActiveMemberPlan is not null)
            {
                viewModel.MembershipStartDate = ActiveMemberPlan.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = ActiveMemberPlan.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberPlan.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null)
                return null;

            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);   
            if (Member is null)
                return null;
            return _mapper.Map<MemberToUpdateViewModel>(Member);
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try 
            {
                var MemberRepo = _unitOfWork.GetRepository<Member>();

                var IsEmailExists = MemberRepo.GetAll(E =>E.Email == memberToUpdate.Email && E.Id != id);
                var IsPhoneExists = MemberRepo.GetAll(P =>P.Phone == memberToUpdate.Phone && P.Id != id);
                if (IsEmailExists.Any() || IsPhoneExists.Any())
                    return false;

                var Member = MemberRepo.GetById(id);
                if (Member is null) 
                    return false;
                _mapper.Map(memberToUpdate,Member);
                MemberRepo.Update(Member);
                return _unitOfWork.SaveChange() > 0;
            } 
            catch
            {
                return false;
            }
        }


        public bool RenewMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var MemberPlanRepo = _unitOfWork.GetRepository<MemberPlan>();
            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) 
                return false;

            var HasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                                              .GetAll(M =>M.MemberId == MemberId && M.Session.StartDate > DateTime.Now).Any();
            if(HasActiveMemberSessions)
                return false;

            var MemberShips = MemberPlanRepo.GetAll( M =>M.MemberId == MemberId);
            try
            {
                if(MemberShips.Any())
                    foreach(var memberShips in MemberShips)
                        MemberPlanRepo.Delete(memberShips);
                MemberRepo.Delete(Member) ;
                return _unitOfWork.SaveChange() >0 ;
            } 
            catch
            {
                return false;
            }

        }


        //Help Methods
        private bool IsEmailExists(string email) => _unitOfWork.GetRepository<Member>().GetAll(E => E.Email == email).Any();
        private bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Member>().GetAll(E => E.Phone == phone).Any();

    }
}
