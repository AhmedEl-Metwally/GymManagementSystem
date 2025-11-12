using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
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
            return View( plan);
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "InValid plan id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData","Check Data Validation");
                return View(updatePlan);
            }

            bool result = _planService.UpdatePlan(id,updatePlan);
            if(result)
                TempData["SuccessMessage"] = "Plan success To Update";
            else
                TempData["ErrorMessage"] = "Plan Failed To Update";
            return RedirectToAction(nameof(Index));
        }
    }
}
