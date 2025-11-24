using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberPlanViewModels;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class MemberSessionService(IUnitOfWork _unitOfWork,IMapper _mapper) : IMemberSessionService
    {
        public IEnumerable<SessionViewModel> GetAllMemberSessionWithTrainerAndCategory()
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var sessions = sessionRepository.GetAllSessionsWithTrainerAndCategory();
            var sessionView = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in sessionView)
                session.AvailableSlots = session.Capacity - sessionRepository.GetCountOfMemberSessionSlots(session.Id);    

            return sessionView;
        }

        public IEnumerable<MemberForSessionViewModel> GetAllMemberSession(int sessionId)
        {
            var memberSessionRepository = _unitOfWork.MemberSessionRepository;
            var memberSession= memberSessionRepository.GetMemberSessionById(sessionId);
            var memberForSessionViewMode = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(memberSession);
            return memberForSessionViewMode;
        }

        public bool CreateMemberSession(CreateMemberSessionViewModel createMemberSessionViewModel)
        {
            var session = _unitOfWork.SessionRepository.GetById(createMemberSessionViewModel.SessionId);
            if(session is null || session.StartDate <= DateTime.UtcNow)
                return false;

            var memberSessionRepository = _unitOfWork.MemberPlanRepository;
            var activeMemberSessionForMember = memberSessionRepository.GetFirstOrDefault(M => M.Status.ToLower() == "active" && M.MemberId == createMemberSessionViewModel.MemberId);  
            if(activeMemberSessionForMember is null) 
                  return false;

            var sessionRepository = _unitOfWork.SessionRepository;
            var memberForSession = sessionRepository.GetCountOfMemberSessionSlots(createMemberSessionViewModel.SessionId);
            var availableSlots = session.Capacity - memberForSession;
            if(availableSlots == 0)
                return false;

            var memberSession = _mapper.Map<MemberSession>(createMemberSessionViewModel);
            memberSession.IsAttended = false;
            _unitOfWork.MemberSessionRepository.Add(memberSession);
            return _unitOfWork.SaveChange() > 0;
        }

        public IEnumerable<MemberForSelectListViewModel> GetMembersForDropdown(int sessionId)
        {
            var memberSessionRepository = _unitOfWork.MemberSessionRepository;
            var memberSessionIds = memberSessionRepository.GetAll(S => S.Id == sessionId)
                                                                   .Select(MS => MS.MemberId)
                                                                   .ToList();

            var memberAvailableToMemberSession = _unitOfWork.GetRepository<Member>().GetAll(M => !memberSessionIds.Contains(M.Id));
            var memberSessionSelectList = _mapper.Map<IEnumerable<MemberForSelectListViewModel>>(memberAvailableToMemberSession);
            return memberSessionSelectList;
        }
    }
}
