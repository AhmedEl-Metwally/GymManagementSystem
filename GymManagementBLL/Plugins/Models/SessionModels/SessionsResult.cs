namespace GymManagementBLL.Plugins.Models.SessionModels
{
    public record SessionsResult(string Period, int TotalMatched, IEnumerable<SessionInfo> Sessions, string? Note = null);

}
