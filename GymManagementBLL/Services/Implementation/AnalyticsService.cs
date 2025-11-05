using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var sessions = _unitOfWork.SessionRepository.GetAll();

            return new AnalyticsViewModel 
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberPlan>().GetAll(M =>M.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),    
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = sessions.Count(S => S.StartDate > DateTime.Now),
                CompletedSessions = sessions.Count(S => S.EndDate < DateTime.Now),
                OngoingSessions = sessions.Count(S => S.StartDate <= DateTime.Now && S.EndDate >= DateTime.Now)
            };
        }
    }
}
