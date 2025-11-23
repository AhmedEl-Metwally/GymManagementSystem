using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
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
                session.AvailableSlots = session.Capacity - sessionRepository.GetCountOfBookedSlots(session.Id);    

            return sessionView;
        }

        public IEnumerable<MemberForSessionViewModel> GetAllMemberSession(int sessionId)
        {
            var memberSessionRepository = _unitOfWork.MemberSessionRepository;
            var memberSession= memberSessionRepository.GetMemberSessionById(sessionId);
            var memberForSessionViewMode = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(memberSession);
            return memberForSessionViewMode;
        }

    }
}
