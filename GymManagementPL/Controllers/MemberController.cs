using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 OR Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetails(id);
            if(Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 OR Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var HealthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
                
            return View(HealthRecord);
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMember(CreateMemberViewModel createMember) 
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid","Check Data And Missing Fields");
                return View(nameof(Create),createMember);
            }

            bool Result = _memberService.CreateMember(createMember);
            if (Result)
                TempData["SuccessMessage"] = "Member Create Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Create , Check Phone And Email";

            return RedirectToAction(nameof(Index));
        }


    }
}
