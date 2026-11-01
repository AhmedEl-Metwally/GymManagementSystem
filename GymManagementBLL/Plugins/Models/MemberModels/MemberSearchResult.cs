namespace GymManagementBLL.Plugins.Models.MemberModels
{
    public record MemberSearchResult(int TotalMatched, IEnumerable<MemberStatusInfo> Members, string? Note = null);

}
