namespace GymManagementPL.Extensions
{
    public static class GymApplicationSeedingExtensions
    {
        public static WebApplication SeedGymData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.SeedGymDatabase();
            scope.ServiceProvider.SeedIdentity();
            return app;
        }
    }
}
