namespace GymManagementBLL.Plugins.Models.Plugins
{
    public class PlansPlugin(IPlanService _planService)
    {
        [KernelFunction("get_membership_plans")]
        [Description("Returns every membership PLAN (package) with its name, price in EGP, duration in days and whether it is active. Use it only for questions about plans, packages and prices. Never use it to look up a person.")]
        public IEnumerable<PlanSummary> GetMembershipPlans() => _planService.GetAllPlans().Select(P => new PlanSummary(P.Name, P.Price, P.DurationDays, P.IsActive)).ToList();

        [KernelFunction("find_membership_plan")]
        [Description("Finds membership PLANS (packages) whose name contains the given text and returns their full details. Only for plan names. Never use it to look up a member or a person.")]
        public IEnumerable<PlanInfo> FindMembershipPlans([Description("The plan name, or part of it. Must not be empty.")] string planName)
        {
            if (string.IsNullOrWhiteSpace(planName))
                return [];
            return _planService.GetAllPlans().Where(P => P.Name.Contains(planName, StringComparison.OrdinalIgnoreCase)).Select(ToPlanInfo).ToList();
        }

        // Helper method
        private static PlanInfo ToPlanInfo(PlanViewModel plan)
            => new PlanInfo(plan.Name, plan.Description, plan.Price, plan.DurationDays, plan.IsActive);
    }
}
