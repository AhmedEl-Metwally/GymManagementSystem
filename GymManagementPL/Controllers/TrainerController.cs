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

        public ActionResult GetTrainer()
        {
            return View();
        }

        public ActionResult CreateTrainer()
        {
            return View();
        }
    }
}
