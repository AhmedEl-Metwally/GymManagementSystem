namespace GymManagementBLL.Extensions
{
    public static class GymAutoMapperCollectionExtensions
    {
        public static IServiceCollection AddGymAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Mapping => Mapping.AddProfile(new MappingProfile()));

            return services;
        }
    }
}
