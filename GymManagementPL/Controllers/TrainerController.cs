using GymManagementBLL.Services.Interface;
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
            if(id <= 0)
              return RedirectToAction(nameof(Index));

            var trainer = _trainerService.GetTrainerDetails(id);
            if(trainer is null)
              return RedirectToAction(nameof(Index));
            return View(trainer);
        }

        public ActionResult CreateTrainer()
        {
            return View();
        }
    }
}
