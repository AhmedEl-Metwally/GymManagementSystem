using GymManagementBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberSessionController(IMemberSessionService _memberSessionService) : Controller
    {
        public IActionResult Index()
        {
            var sessions = _memberSessionService.GetAllMemberSessionWithTrainerAndCategory();
            return View(sessions);
        }

        public IActionResult GetMemberForUpcomingSession(int sessionId)
        {
            var members = _memberSessionService.GetAllMemberForUpcomingSession(sessionId);
            return View(members);
        }
    }
}
