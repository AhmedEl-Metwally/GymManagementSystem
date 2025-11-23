using GymManagementBLL.ViewModels.MemberSessionViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberSessionService
    {
        IEnumerable<SessionViewModel> GetAllMemberSessionWithTrainerAndCategory();
        IEnumerable<MemberForSessionViewModel> GetAllMemberSession(int sessionId);
    }
}
