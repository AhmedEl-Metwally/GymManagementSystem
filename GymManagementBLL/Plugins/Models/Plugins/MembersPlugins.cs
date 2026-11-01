namespace GymManagementBLL.Plugins.Models.Plugins
{
    public class MembersPlugins(IMemberService _memberService, IMemberPlanService _memberPlanService, IAnalyticsService _analyticsService)
    {
        private const int MaxResults = 10;
        private const int MaxMemberMatches = 5;

        [KernelFunction("get_membership_overview")]
        [Description("Returns the gym's member counts: total members, active memberships, and members without an active membership. Use it for questions such as how many members the gym has.")]
        public MemberShipOverview GetMemberShipOverview()
        {
            var data = _analyticsService.GetAnalyticsData();
            return new MemberShipOverview(data.TotalMembers, data.ActiveMembers, Math.Max(0, data.TotalMembers - data.ActiveMembers));
        }

        [KernelFunction("get_expiring_memberships")]
        [Description("Lists active subscriptions that end soon across ALL members, soonest first: member name, plan name, end date and days left. Use it for questions such as whose subscription is about to expire. Not for checking one specific member; use find_member for that.")]
        public ExpiringMemberShipsResult GetExpiringMemberShips([Description("Days to look ahead, 1 to 60. 'this week' (الأسبوع ده) = 7. 'this month' (الشهر ده) = 30. Use 7 if the user does not say.")] int days = 7)
        {
            var normalizedDays = Math.Clamp(days, 1, 60);
            var now = DateTime.Now;
            var limit = now.AddDays(normalizedDays);
            var matched = _memberPlanService.GetAllMemberPlans().Where(M => M.EndDate <= limit).OrderBy(M => M.EndDate).ToList();
            var memberShip = matched.Take(MaxResults).Select(M => new ExpiringMemberShipInfo(M.MemberName, M.PlanName, M.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), Math.Max(0, (M.EndDate.Date - now.Date).Days))).ToList();
            var note = matched.Count == 0 ? $"No active membership ends within {normalizedDays} days." : null;
            return new ExpiringMemberShipsResult(normalizedDays, matched.Count, memberShip, note);
        }

        [KernelFunction("find_member")]
        [Description("Looks up ONE gym member (a person) by name and returns whether that person has an active subscription, with the plan name and end date. Use it for questions about a specific member's subscription.")]
        public MemberSearchResult FindMemberOfMembership([Description("The member's name or part of it, at least 2 characters, exactly as the user wrote it.")] string memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName) || memberName.Trim().Length < 2)
                return new MemberSearchResult(0, [], "Provide at least 2 characters of the member's name.");

            var name = memberName.Trim();
            var matchedMembers = _memberService.GetAllMembers().Where(M => M.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            var activeByMember = _memberPlanService.GetAllMemberPlans()
                                                                .GroupBy(P => P.MemberId)
                                                                .ToDictionary(P => P.Key, P => P.OrderByDescending(P => P.EndDate).First());
            var members = matchedMembers.Take(MaxMemberMatches)
                                                        .Select(M => activeByMember.TryGetValue(M.Id, out var plan)
                                                        ? new MemberStatusInfo(M.Name, true, plan.PlanName, plan.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                                                        : new MemberStatusInfo(M.Name, false, null, null)).ToList();
            var note = matchedMembers.Count == 0 ? "No member found with this name." : null;
            return new MemberSearchResult(matchedMembers.Count, members, note);
        }
    }
}
