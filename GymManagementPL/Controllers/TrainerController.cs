namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController(IServiceManager _serviceManager) : Controller
    {
        public ActionResult Index()
        {
            var trainers = _serviceManager.TrainerService.GetAllTrainers();
            return View(trainers);
        }

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can Not Be 0 OR Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _serviceManager.TrainerService.GetTrainerDetails(id);
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
            bool result = _serviceManager.TrainerService.CreateTrainer(createTrainer);
            if (result)
                TempData["SuccessMessage"] = "Trainer Create Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Create , Check Phone And Email";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can Not Be 0 OR Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _serviceManager.TrainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = " Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(trainerToUpdate));
            }

            bool result = _serviceManager.TrainerService.UpdateTrainerDetails(trainerToUpdate, id);
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

            var trainer = _serviceManager.TrainerService.GetTrainerDetails(id);
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
            bool result = _serviceManager.TrainerService.RemoveTrainer(id);
            if (result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Delete Trainer";

            return RedirectToAction(nameof(Index));

        }
    }
}
