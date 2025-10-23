using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class SessionService(IUnitOfWork _unitOfWork,IMapper _mapper) : ISessionService
    {
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.GetRepository<Session>().GetAll();
            if (!sessions.Any())
                return [];

            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
                    session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int SessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWhithTrainerAndCategory(SessionId);
            if(session is null)
                return null;

            var mappedSession = _mapper.Map<Session,SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(mappedSession.Id);
            return mappedSession;
        }



    }
}
