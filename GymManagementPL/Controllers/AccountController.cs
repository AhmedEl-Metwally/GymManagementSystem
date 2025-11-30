using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController(IAccountService _accountService, SignInManager<ApplicationUser> _signInManager) : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel) 
        {
            if(!ModelState.IsValid)
                return View(loginViewModel);

            var user = _accountService.ValidateUser(loginViewModel);
            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(loginViewModel);
            }

            var result = _signInManager.PasswordSignInAsync(user,loginViewModel.Password,loginViewModel.RememberMe,false).Result;
            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Account Not Allowed");
            if(result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Account Locked");
            if(result.Succeeded)
            return RedirectToAction(nameof(Index), "Home");

            return View(loginViewModel);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
    }
}
