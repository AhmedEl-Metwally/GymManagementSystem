namespace GymManagementBLL.Plugins.Models.Plugins
{
    public class SessionsPlugin(ISessionService _sessionService)
    {
        private const int MaxSessions = 8;

        [KernelFunction("get_sessions")]
        [Description("Returns the gym's training sessions for a time period, ordered by start time. Each session has: category, trainer name, date, start and end time (24h), status, capacity and available slots. Use it for questions about which sessions exist, which have free places, or when the next session is, Only for the schedule of training sessions. Never use it for trainers, members or plans.")]
        public SessionsResult GetSessions([Description("Time period. Must be exactly one of: today, tomorrow, next_7_days, upcoming. today = اليوم / النهارده. tomorrow = بكرة / غدا. next_7_days = الأسبوع ده / هذا الأسبوع. upcoming = القادمة / أقرب جلسة / the next session.")] string period, [Description("Optional. Fill it ONLY if the user explicitly names a session category. Otherwise leave it empty.")] string? category = null)
        {
            var now = DateTime.Now;
            var normalizedPeriod = (period ?? string.Empty).Trim().ToLowerInvariant();

            if (!GetTryGetRange(normalizedPeriod, now, out var from, out var to))
                return new SessionsResult(normalizedPeriod, 0, [], "Unknown period. Use one of: today, tomorrow, next_7_days, upcoming.");

            var matched = _sessionService.GetAllSessions()
                                            .Where(S => S.StartDate < to && S.EndDate > from)
                                            .Where(S => string.IsNullOrWhiteSpace(category) || S.CategoryName.Contains(category, StringComparison.OrdinalIgnoreCase))
                                            .OrderBy(S => S.StartDate)
                                            .ToList();

            var session = matched.Take(MaxSessions).Select(ToSessionInfo).ToList();
            var note = matched.Count == 0 ? "No sessions found for this period and category." : null;
            return new SessionsResult(normalizedPeriod, matched.Count, session, note);
        }


        // Helper method
        private static bool GetTryGetRange(string period, DateTime now, out DateTime from, out DateTime to)
        {
            switch (period)
            {
                case "today":
                    (from, to) = (now.Date, now.Date.AddDays(1));
                    return true;
                case "tomorrow":
                    (from, to) = (now.Date.AddDays(1), now.Date.AddDays(2));
                    return true;
                case "next_7_days":
                    (from, to) = (now, now.Date.AddDays(7));
                    return true;
                case "upcoming":
                    (from, to) = (now, DateTime.MaxValue);
                    return true;
                default:
                    (from, to) = (default, default);
                    return false;
            }
        }

        private static SessionInfo ToSessionInfo(SessionViewModel session)
            => new
            (
         session.CategoryName, session.TrainerName,
            session.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
           session.StartDate.ToString("HH:mm", CultureInfo.InvariantCulture),
            session.EndDate.ToString("HH:mm", CultureInfo.InvariantCulture),
                session.Status, session.Capacity, session.AvailableSlots
            );
    }
}
