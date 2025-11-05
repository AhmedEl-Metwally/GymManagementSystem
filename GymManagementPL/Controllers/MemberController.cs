using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
