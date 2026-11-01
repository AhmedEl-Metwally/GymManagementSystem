namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PlanController(IServiceManager _serviceManager) : Controller
    {
        public ActionResult Index()
        {
            var plans = _serviceManager.PlanService.GetAllPlans();
            return View(plans);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "InValid plan id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _serviceManager.PlanService.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "No plan Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "InValid plan id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _serviceManager.PlanService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatePlan);
            }

            bool result = _serviceManager.PlanService.UpdatePlan(id, updatePlan);
            if (result)
                TempData["SuccessMessage"] = "Plan success To Update";
            else
                TempData["ErrorMessage"] = "Plan Failed To Update";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id)
        {
            bool result = _serviceManager.PlanService.ToggleStatus(id);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed TO Change Plan";
            return RedirectToAction(nameof(Index));

        }
    }
}
