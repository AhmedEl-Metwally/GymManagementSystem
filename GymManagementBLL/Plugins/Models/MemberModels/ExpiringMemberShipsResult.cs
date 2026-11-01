namespace GymManagementBLL.Plugins.Models.MemberModels
{
    public record ExpiringMemberShipsResult(int WithInDays, int TotalMatched, IEnumerable<ExpiringMemberShipInfo> MemberShips, string? Note = null);

}
