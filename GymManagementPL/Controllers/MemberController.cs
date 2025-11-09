using GymManagementBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController(IMemberService _memberService) : Controller
    {
        public ActionResult Index()
        {
            var member = _memberService.GetAllMembers();
            return View(member);
        }

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));

            var Member = _memberService.GetMemberDetails(id);
            if(Member is null)
                return RedirectToAction(nameof(Index));

            return View(Member);
        }

        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
