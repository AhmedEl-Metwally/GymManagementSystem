namespace GymManagementPL.Extensions
{
    public static class FunctionFilterCollectionExtensions
    {
        public static IServiceCollection AddFunctionFilters(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IFunctionInvocationFilter, FunctionLoggingFilter>();
            services.AddScoped<IFunctionInvocationFilter, ToolAuthorizationFilter>();

            return services;
        }
    }
}
