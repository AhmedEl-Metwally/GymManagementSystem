using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
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
            LoadDropDownsForCategory();
            LoadDropDownsForTrainer();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(CreateSessionViewModel CreateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForCategory();
                LoadDropDownsForTrainer();
                return View(nameof(Create), CreateSession);
            }

            bool result = _sessionService.CreateSession(CreateSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                LoadDropDownsForCategory();
                LoadDropDownsForTrainer();
                TempData["ErrorMessage"] = "Session Failed To Create";
                return View(CreateSession);
            }
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0) 
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null) 
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsForTrainer();
            return View(session);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Edit([FromRoute] int SessionId, UpdateSessionViewModel UpdateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainer();
                return View(UpdateSession);
            }

            var result = _sessionService.UpdateSession(UpdateSession,SessionId);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
            }
            else 
            {
                TempData["ErrorMessage"] = "Session Failed To Update";
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var result = _sessionService.GetSessionById(id);

            if (result is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool result = _sessionService.RemoveSession(id);
            if (result)
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Session Failed To Delete";

            return RedirectToAction(nameof(Index));
        }



        // Helper Methods

        private void LoadDropDownsForTrainer()
        {
            var trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.trainers = new SelectList(trainers, "Id", "Name");
        }

        private void LoadDropDownsForCategory()
        {
            var categories = _sessionService.GetCategoryForDropDown();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
        }

    }
}
