namespace GymManagementPL.Extensions
{
    public static class IdentitySeedingExtensions
    {
        public static void SeedIdentity(this IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            IdentityDbContextSeeding.SeedData(roleManager, userManager);
        }
    }
}
