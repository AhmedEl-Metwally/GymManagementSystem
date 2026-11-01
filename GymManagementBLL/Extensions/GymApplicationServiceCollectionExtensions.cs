namespace GymManagementBLL.Extensions
{
    public static class GymApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddGymApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IMemberPlanService, MemberPlanService>();
            services.AddScoped<IMemberSessionService, MemberSessionService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IGymAssistantService, GymAssistantService>();
            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        }
    }
}
