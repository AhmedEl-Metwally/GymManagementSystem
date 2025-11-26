using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var members = _memberSessionService.GetAllMemberSession(sessionId);
            return View(members);
        }

        public IActionResult GetMemberForOngoingSession(int sessionId)
        {
            var members = _memberSessionService.GetAllMemberSession(sessionId);
            return View(members);
        }

        public IActionResult Create(int id)
        {
            var members = _memberSessionService.GetMembersForDropdown(id);
            var membersSelectList = new SelectList(members,"Id","Name");
            ViewBag.Members = membersSelectList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMemberSessionViewModel createMemberSessionViewModel)
        {
           var result = _memberSessionService.CreateMemberSession(createMemberSessionViewModel);
            if (result)
                TempData["SuccessMessage"] = "Member Session Created successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Create Member Session.";

            return RedirectToAction(nameof(GetMemberForOngoingSession), new { sessionId = createMemberSessionViewModel.SessionId });
        }
    }
}
