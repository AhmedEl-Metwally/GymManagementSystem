namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController(IServiceManager _serviceManager) : Controller
    {
        public IActionResult Index()
        {
            var Date = _serviceManager.AnalyticsService.GetAnalyticsData();
            return View(Date);
        }
    }
}
