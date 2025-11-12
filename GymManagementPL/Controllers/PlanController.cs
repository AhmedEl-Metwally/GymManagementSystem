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

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "InValid plan id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "No plan Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
