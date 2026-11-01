namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class MemberPlanController(IServiceManager _serviceManager) : Controller
    {
        public IActionResult Index()
        {
            var memberPlan = _serviceManager.MemberPlanService.GetAllMemberPlans();
            return View(memberPlan);
        }

        public IActionResult Create()
        {
            LoadDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMemberPlanViewModel createMemberPlanView)
        {
            if (ModelState.IsValid)
            {
                var result = _serviceManager.MemberPlanService.CreateMemberPlan(createMemberPlanView);
                if (result)
                {
                    TempData["SuccessMessage"] = "Membership created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Membership can not be created";
                    return RedirectToAction("Index");
                }
            }
            TempData["ErrorMessage"] = "Creation failed, Check the data";
            LoadDropDownList();
            return View(createMemberPlanView);
        }


        public IActionResult CancelMemberPlan(int id)
        {
            var result = _serviceManager.MemberPlanService.DeleteMemberPlan(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership cancelled successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Membership can not be cancelled";
                return RedirectToAction(nameof(Index));
            }
        }

        //Helper Methods

        private void LoadDropDownList()
        {
            var members = _serviceManager.MemberPlanService.GetMemberForDropdown();
            var plans = _serviceManager.MemberPlanService.GetPlanForDropdown();

            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.plans = new SelectList(plans, "Id", "Name");
        }

    }
}
