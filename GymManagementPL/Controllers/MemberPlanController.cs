using GymManagementBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberPlanController(IMemberPlanService _memberPlanService) : Controller
    {
        public IActionResult Index()
        {
            var memberPlan = _memberPlanService.GetAllMemberPlans();
            return View(memberPlan);
        }
    }
}
