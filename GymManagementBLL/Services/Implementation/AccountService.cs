namespace GymManagementBLL.Services.Implementation
{
    public class AccountService(UserManager<ApplicationUser> _userManager) : IAccountService
    {
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (user is null)
                return null;

            var isPasswordValid = _userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
            return isPasswordValid ? user : null;
        }
    }
}
