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

        public ActionResult TrainerEdit(int id) 
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can Not Be 0 OR Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerToUpdate(id);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = " Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainerEdit([FromRoute]int id,TrainerToUpdateViewModel trainerToUpdate)
        {
            if (!ModelState.IsValid)
                return View(nameof(trainerToUpdate));

            bool result = _trainerService.UpdateTrainerDetails(trainerToUpdate,id);
            if (result)
                TempData["SuccessMessage"] = "Trainer Update Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Updated";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id) 
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

            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            bool result = _trainerService.RemoveTrainer(id);
            if(result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Delete Trainer";

            return RedirectToAction(nameof(Index));

        }
    }
}
