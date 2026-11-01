using GymManagementBLL.Plugins.Models.Plugins;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Diagnostics;
using System.Security.Claims;

namespace GymManagementBLL.Services.AI
{
    public class GymAssistantService(Kernel _kernel, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor, IMemoryCache _memoryCache, PlansPlugin _plansPlugin, TrainersPlugin _trainersPlugin, SessionsPlugin _sessionsPlugin, MembersPlugins _membersPlugins, ILogger<GymAssistantService> _logger) : IGymAssistantService
    {
        private const int MaxHistoryMessages = 4;
        private static readonly TimeSpan ConversationLifeTime = TimeSpan.FromMinutes(30);
        private const string FallBackReply = "مش قادر أجاوب دلوقتي، جرّب تعيد السؤال. / I couldn't answer, please try again.";

        public async Task<string> AskAsync(string userMessage, CancellationToken cancellationToken = default)
        {
            userMessage = userMessage.Trim();
            var stopWatch = Stopwatch.StartNew();
            _logger.LogInformation("AI Request START [{Time}] : {Message}", DateTime.Now.ToString("HH:mm:ss.fff"), userMessage);

            RegisterPlugins();

            var systemPrompt = await LoadSystemPromptAsync(cancellationToken);
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            var history = new ChatHistory(systemPrompt);
            foreach (var turn in GetConversation())
            {
                if (turn.Role == "user")
                    history.AddUserMessage(turn.Content);
                else
                    history.AddAssistantMessage(turn.Content);
            }
            history.AddUserMessage(userMessage);
            _logger.LogInformation("AI chat history messages: {Count}", history.Count);

            var settings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0.2,
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };
            var response = await chatService.GetChatMessageContentAsync(history, executionSettings: settings, kernel: _kernel, cancellationToken: cancellationToken);
            var reply = response.Content ?? string.Empty;

            stopWatch.Stop();
            _logger.LogInformation("AI Request END [{Time}] : total {Elapsed} ms , reply Length {Length} chars", DateTime.Now.ToString("HH:mm:ss.fff"), stopWatch.ElapsedMilliseconds, reply.Length);

            if (string.IsNullOrWhiteSpace(reply) || reply.Contains("tool_call", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("AI reply was empty or contained raw tool-call text. It was not shown or saved.");
                return FallBackReply;
            }

            SaveTurn(userMessage, reply);
            return reply;
        }

        public IReadOnlyList<ChatTurn> GetConversation()
        {
            var conversation = FindConversation();
            if (conversation is null)
                return [];
            lock (conversation)
                return conversation.ToList();
        }

        public void ClearConversation()
        {
            var userId = GetCurrentUserId();
            if (userId is not null)
                _memoryCache.Remove(CacheKey(userId));
        }


        // Helper method
        private void RegisterPlugins()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (AiToolPolicy.IsAllowed(AiToolPolicy.Plans, user))
                _kernel.Plugins.AddFromObject(_plansPlugin, AiToolPolicy.Plans);
            if (AiToolPolicy.IsAllowed(AiToolPolicy.Trainers, user))
                _kernel.Plugins.AddFromObject(_trainersPlugin, AiToolPolicy.Trainers);
            if (AiToolPolicy.IsAllowed(AiToolPolicy.Sessions, user))
                _kernel.Plugins.AddFromObject(_sessionsPlugin, AiToolPolicy.Sessions);
            if (AiToolPolicy.IsAllowed(AiToolPolicy.Members, user))
                _kernel.Plugins.AddFromObject(_membersPlugins, AiToolPolicy.Members);

            _logger.LogInformation("AI plugins registered for '{User}' : {Plugins}", user?.Identity?.Name ?? "Unknown", string.Join(",", _kernel.Plugins.Select(P => P.Name)));
        }

        private async Task<string> LoadSystemPromptAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "AI", "Prompts", "system-prompt.txt");
            if (!File.Exists(path))
                throw new FileNotFoundException($"System prompt file not found at {path}");
            return await File.ReadAllTextAsync(path, cancellationToken);
        }

        private void SaveTurn(string userMessage, string reply)
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return;

            var conversation = _memoryCache.GetOrCreate(CacheKey(userId), E =>
            {
                E.SlidingExpiration = ConversationLifeTime;
                return new List<ChatTurn>();
            })!;

            lock (conversation)
            {
                conversation.Add(new ChatTurn("user", userMessage));
                conversation.Add(new ChatTurn("assistant", reply));

                if (conversation.Count > MaxHistoryMessages)
                    conversation.RemoveRange(0, conversation.Count - MaxHistoryMessages);
            }

        }

        private string? GetCurrentUserId() => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        private static string CacheKey(string userId) => $"ai_chat: {userId}";

        private List<ChatTurn>? FindConversation()
        {
            var userId = GetCurrentUserId();
            return userId is not null && _memoryCache.TryGetValue(CacheKey(userId), out List<ChatTurn>? conversation) ? conversation : null;
        }

    }
}
