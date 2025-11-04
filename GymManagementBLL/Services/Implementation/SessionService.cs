using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

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

        public bool CreateSession(CreateSessionViewModel CreateSession)
        {
            try
            {
                if (!IsTrainerExists(CreateSession.TrainerId))
                    return false;
                if (!IsCategoryExists(CreateSession.CategoryId))
                    return false;
                if (!IsDateTimeValid(CreateSession.StartDate, CreateSession.EndDate))
                    return false;
                if (CreateSession.Capacity > 25 || CreateSession.Capacity < 0)
                    return false;

                var SessionEntity = _mapper.Map<Session>(CreateSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChange() > 0;

            }
            catch
            {
                return false;
            }
        }



        // Helper Methods

        private bool IsTrainerExists(int TrainerId) => _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;

        private bool IsCategoryExists(int CategoryId) => _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate) => StartDate < EndDate;
    }
}
