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

        public ActionResult GetMember()
        {
            return View();
        }

        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
