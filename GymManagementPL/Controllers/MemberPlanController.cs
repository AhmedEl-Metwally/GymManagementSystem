using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberPlanViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberPlanController(IMemberPlanService _memberPlanService) : Controller
    {
        public IActionResult Index()
        {
            var memberPlan = _memberPlanService.GetAllMemberPlans();
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
                var result = _memberPlanService.CreateMemberPlan(createMemberPlanView);
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
            var result = _memberPlanService.DeleteMemberPlan(id);
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
            var members = _memberPlanService.GetMemberForDropdown();
            var plans = _memberPlanService.GetPlanForDropdown();

            ViewBag.Members = new SelectList(members,"Id","Name");
            ViewBag.plans = new SelectList(plans, "Id","Name");
        }

    }
}
