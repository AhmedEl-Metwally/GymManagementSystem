using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberSessionController(IMemberSessionService _memberSessionService) : Controller
    {
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            var sessions = _memberSessionService.GetAllMemberSessionWithTrainerAndCategory();
            return View(sessions);
        }

        public IActionResult GetMemberForUpcomingSession(int sessionId)
        {
            var members = _memberSessionService.GetAllMemberSession(sessionId);
            ViewBag.SessionId = sessionId;
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
            ViewBag.SessionId = id;
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

            return RedirectToAction(nameof(GetMemberForUpcomingSession), new { sessionId = createMemberSessionViewModel.SessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MemberAttended(MemberAttendOrCancelViewModel memberAttendOrCancel)
        {
            var result = _memberSessionService.MemberAttended(memberAttendOrCancel);
            return RedirectToAction(nameof(GetMemberForOngoingSession), new { sessionId = memberAttendOrCancel.SessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MemberCancel(MemberAttendOrCancelViewModel memberAttendOrCancel)
        {
            var result = _memberSessionService.CancelMemberSession(memberAttendOrCancel);
            if (result)
                TempData["SuccessMessage"] = "Member cancelled successfully!";
            else
                TempData["ErrorMessage"] = "Failed to mark Member as cancelled.";
            return RedirectToAction(nameof(GetMemberForUpcomingSession), new { sessionId = memberAttendOrCancel.SessionId });
           
        }


    }
}
