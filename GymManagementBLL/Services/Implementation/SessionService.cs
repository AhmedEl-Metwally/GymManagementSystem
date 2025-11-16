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
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(SessionId);
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

        public UpdateSessionViewModel? GetSessionToUpdate(int SessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(SessionId);
            if(!IsSessionAvailableForUpdate(Session!))
                return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel UpdateSession, int SessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(SessionId);
                if(!IsSessionAvailableForUpdate(Session!))
                    return false;
                if(!IsTrainerExists(UpdateSession.TrainerId))
                    return false;
                if(!IsDateTimeValid(UpdateSession.StartDate,UpdateSession.EndDate))
                    return false;

                _mapper.Map(UpdateSession,Session);
                Session!.UpdatedAt = DateTime.Now;  
                _unitOfWork.SessionRepository.Update(Session);
                return _unitOfWork.SaveChange() > 0;
            }
            catch 
            {
                return false;
            }
        }

        public bool RemoveSession(int SessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(SessionId);
                if(!IsSessionAvailableForRemove(Session!))
                    return false;
                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChange() >0;    
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }


        // Helper Methods

        private bool IsTrainerExists(int TrainerId) => _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        private bool IsCategoryExists(int CategoryId) => _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
       private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate) => EndDate > StartDate && DateTime.Now < StartDate;
        // private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate) => StartDate < EndDate && DateTime.Now < StartDate;

        private bool IsSessionAvailableForUpdate(Session session)
        {
            //if(session is null)
            //    return false;
            //if(session.StartDate <= DateTime.Now)
            //    return false;
            //if(session.EndDate < DateTime.Now)
            //    return false;

            //var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) >0;
            //if(HasActiveBooking)
            //    return false;

            //return true;

            return session.StartDate > DateTime.Now && _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) == 0;
        }

        private bool IsSessionAvailableForRemove(Session session)
        {
            //if (session is null)
            //    return false;
            //if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now )
            //    return false;
            //if (session.StartDate > DateTime.Now)
            //    return false;

            //var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            //if (HasActiveBooking)
            //    return false;

            //return true;
            return session.StartDate < DateTime.Now && _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) == 0;
        }
    }
}
