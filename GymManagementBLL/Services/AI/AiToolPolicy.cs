namespace GymManagementBLL.Services.AI
{
    public static class AiToolPolicy
    {
        public const string Trainers = "trainers";
        public const string Plans = "plans";
        public const string Sessions = "sessions";
        public const string Members = "members";

        private static readonly Dictionary<string, string[]> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            [Trainers] = ["SuperAdmin"],
            [Plans] = ["SuperAdmin", "Admin"],
            [Sessions] = ["SuperAdmin", "Admin"],
            [Members] = ["SuperAdmin"]
        };

        private static readonly HashSet<string> LogSafeTools = new(StringComparer.OrdinalIgnoreCase)
        {
            "members-get_membership_overview"
        };

        public static bool IsAllowed(string pluginName, ClaimsPrincipal? user)
        {
            if (user?.Identity?.IsAuthenticated != true)
                return false;

            return AllowedRoles.TryGetValue(pluginName, out var roles) && roles.Any(user.IsInRole);
        }

        public static bool IsLogSafe(string toolName)
            => !toolName.StartsWith(Members + "-", StringComparison.OrdinalIgnoreCase) || LogSafeTools.Contains(toolName);

        public static bool IsSensitiveArgument(string argumentName)
            => argumentName.Equals("memberName", StringComparison.OrdinalIgnoreCase);
    }
}
