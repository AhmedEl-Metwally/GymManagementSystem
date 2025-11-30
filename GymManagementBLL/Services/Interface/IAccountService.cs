using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;

namespace GymManagementBLL.Services.Interface
{
    public interface IAccountService
    {
        ApplicationUser? ValidateUser(LoginViewModel loginViewModel);
    }
}
