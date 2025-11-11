using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController(ITrainerService _trainerService) : Controller
    {
        public ActionResult Index()
        {
            var trainers =_trainerService.GetAllTrainers();
            return View(trainers);
        }

        public ActionResult TrainerDetails(int id )
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can Not Be 0 OR Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = " Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), createTrainer);
            }
            bool result = _trainerService.CreateTrainer(createTrainer);
            if(result)
                TempData["SuccessMessage"] = "Trainer Create Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Create , Check Phone And Email";
            return RedirectToAction(nameof(Index));
        }
    }
}
