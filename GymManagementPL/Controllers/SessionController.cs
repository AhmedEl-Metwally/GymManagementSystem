using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController(ISessionService _sessionService) : Controller
    {
        public ActionResult Index()
        {
            var session = _sessionService.GetAllSessions();
            return View(session);
        }

        public ActionResult Details(int id) 
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public ActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(CreateSessionViewModel CreateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns();
                return View(nameof(Create), CreateSession);
            }

            bool result = _sessionService.CreateSession(CreateSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToRoute(nameof(Index));
            }
            else 
            {
                LoadDropDowns();
                TempData["ErrorMessage"] = "Session Failed To Create";
                return View(CreateSession);
            }
        }


        // Helper Methods

        private void LoadDropDowns()
        {
            var trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.trainers = new SelectList(trainers, "Id", "Name");

            var categories = _sessionService.GetCategoryForDropDown();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
        }

    }
}
