namespace GymManagementDAL.Extensions
{
    public static class GymRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddGymRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IMemberPlanRepository, MemberPlanRepository>();
            services.AddScoped<IMemberSessionRepository, MemberSessionRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
