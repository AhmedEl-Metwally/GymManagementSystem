namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AssistantController(IServiceManager _serviceManager, ILogger<AssistantController> _logger) : Controller
    {
        private const int MaxMessageLength = 500;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ask([FromBody] AskRequest request, CancellationToken cancellationToken)
        {
            var message = request?.Message?.Trim();

            if (string.IsNullOrWhiteSpace(message))
                return BadRequest(new { error = "اكتب سؤالك الأول" });
            if (message.Length > MaxMessageLength)
                return BadRequest(new { error = $"السؤال طويل. الحد الأقصى {MaxMessageLength} حرف." });

            try
            {
                var reply = await _serviceManager.GymAssistantService.AskAsync(message, cancellationToken);
                return Json(new { reply });
            }
            catch (OperationCanceledException)
            {
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI assistant request failed");
                return StatusCode(500, new { error = "المساعد مش متاح دلوقتي. جرّب تاني بعد شوية" });
            }
        }

        [HttpGet]
        public IActionResult History() => Json(_serviceManager.GymAssistantService.GetConversation());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            _serviceManager.GymAssistantService.ClearConversation();
            return NoContent();
        }
    }
}
