using GymManagementBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController(IPlanService _planService) : Controller
    {
        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }
    }
}
