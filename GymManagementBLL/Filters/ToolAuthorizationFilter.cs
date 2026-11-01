namespace GymManagementBLL.Filters
{
    public class ToolAuthorizationFilter(IHttpContextAccessor _httpContextAccessor, ILogger<ToolAuthorizationFilter> _logger) : IFunctionInvocationFilter
    {
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var pluginName = context.Function.PluginName ?? string.Empty;

            if (!AiToolPolicy.IsAllowed(pluginName, user))
            {
                _logger.LogWarning("AI tool DENIED :{Plugin}-{Function} for user '{User}' ", pluginName, context.Function.Name, user?.Identity?.Name ?? "Unknown");
                context.Result = new FunctionResult(context.Function, "Access denied. The current user is not allowed to use this tool. Tell the user that you cannot access that data.");
                return;
            }
            await next(context);
        }
    }
}
