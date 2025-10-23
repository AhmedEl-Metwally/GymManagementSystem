using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int SessionId);
   
    }
}
