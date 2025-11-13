using GymManagementBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class SessionController(ISessionService _sessionService) : Controller
    {
        public ActionResult Index()
        {
            var session = _sessionService.GetAllSessions();
            return View(session);
        }
    }
}
