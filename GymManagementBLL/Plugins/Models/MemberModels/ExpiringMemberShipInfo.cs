namespace GymManagementBLL.Plugins.Models.MemberModels
{
    public record ExpiringMemberShipInfo(string Member, string Plan, string EndDate, int DaysLeft);

}
