namespace GymManagementBLL.ViewModels.MemberPlanViewModels
{
    public class MemberPlanViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;

        public int PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
