using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class SessionService(IUnitOfWork _unitOfWork) : ISessionService
    {
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.GetRepository<Session>().GetAll();
            if (!sessions.Any())
                return [];
            return sessions.Select(s =>new SessionViewModel() 
            {
                Id = s.Id,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Category = s.Capacity,
                CategoryName = s.Category.CategoryName,
                TrainerName = s.Trainer.Name, 
                AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id)
            });
        }
    }
}
