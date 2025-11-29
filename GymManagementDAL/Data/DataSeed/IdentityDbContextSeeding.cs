using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
			try
			{
                bool HasUser = userManager.Users.Any();
                bool HasRole = roleManager.Roles.Any();
                if (HasUser && HasRole)
                    return false;

                if (!HasRole)
                {
                    var Role = new List<IdentityRole>()
                    {
                        new(){ Name = "SuperAdmin"},
                        new(){ Name= "Admin"}
                    };

                    foreach (var role in Role)
                    {
                        if(!roleManager.RoleExistsAsync(role.Name!).Result)
                            roleManager.CreateAsync(role).Wait();   
                    }
                }

                if (!HasUser)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Mohamed",
                        UserName = "EntaMalk",
                        Email = "ahmed.moh.elmetwally@gmail.com",
                        PhoneNumber = "01091399362"
                    };
                    userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Khaled",
                        LastName = "Taalab",
                        UserName = "Kh7aled",
                        Email = "Kh7aled@gmail.com",
                        PhoneNumber = "01142133508"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
			catch
			{
                return false;
			}
        }
    }
}
