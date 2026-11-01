namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class MemberSessionController(IServiceManager _serviceManager) : Controller
    {
        public IActionResult Index()
        {
            var sessions = _serviceManager.MemberSessionService.GetAllMemberSessionWithTrainerAndCategory();
            return View(sessions);
        }

        public IActionResult GetMemberForUpcomingSession(int sessionId)
        {
            var members = _serviceManager.MemberSessionService.GetAllMemberSession(sessionId);
            ViewBag.SessionId = sessionId;
            return View(members);
        }

        public IActionResult GetMemberForOngoingSession(int sessionId)
        {
            var members = _serviceManager.MemberSessionService.GetAllMemberSession(sessionId);
            return View(members);
        }

        public IActionResult Create(int id)
        {
            var members = _serviceManager.MemberSessionService.GetMembersForDropdown(id);
            var membersSelectList = new SelectList(members, "Id", "Name");
            ViewBag.Members = membersSelectList;
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMemberSessionViewModel createMemberSessionViewModel)
        {
            var result = _serviceManager.MemberSessionService.CreateMemberSession(createMemberSessionViewModel);
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
            var result = _serviceManager.MemberSessionService.MemberAttended(memberAttendOrCancel);
            return RedirectToAction(nameof(GetMemberForOngoingSession), new { sessionId = memberAttendOrCancel.SessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MemberCancel(MemberAttendOrCancelViewModel memberAttendOrCancel)
        {
            var result = _serviceManager.MemberSessionService.CancelMemberSession(memberAttendOrCancel);
            if (result)
                TempData["SuccessMessage"] = "Member cancelled successfully!";
            else
                TempData["ErrorMessage"] = "Failed to mark Member as cancelled.";
            return RedirectToAction(nameof(GetMemberForUpcomingSession), new { sessionId = memberAttendOrCancel.SessionId });

        }


    }
}
