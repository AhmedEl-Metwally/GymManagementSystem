namespace GymManagementDAL.Extensions
{
    public static class GymDbContextServiceExtensions
    {
        public static IServiceCollection AddGymDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
            });
            return services;
        }
    }
}
