using AutoMapper;
using GymManagementBLL.Services.Interface;
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
    }
}
