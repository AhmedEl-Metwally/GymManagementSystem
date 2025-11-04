using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int SessionId);
        bool CreateSession(CreateSessionViewModel CreateSession);
        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);
        bool UpdateSession(UpdateSessionViewModel UpdateSession,int SessionId);
    }
}
