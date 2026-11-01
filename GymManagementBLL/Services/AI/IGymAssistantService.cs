namespace GymManagementBLL.Services.AI
{
    public interface IGymAssistantService
    {
        Task<string> AskAsync(string userMessage, CancellationToken cancellationToken = default);
        IReadOnlyList<ChatTurn> GetConversation();
        void ClearConversation();
    }
}
