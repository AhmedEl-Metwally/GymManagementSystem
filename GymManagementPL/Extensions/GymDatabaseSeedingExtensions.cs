namespace GymManagementPL.Extensions
{
    public static class GymDatabaseSeedingExtensions
    {
        public static void SeedGymDatabase(this IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<GymDbContext>();
            GymDbContextSeeding.SeedData(dbContext);
        }
    }
}




