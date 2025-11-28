using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserStore<AppUser> _userStore;

        public UserController(IDbContextFactory<AppDBContext> context,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager, UserService userService) : base(context, userService)
        {
            _userStore = userStore;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel
            {
                UserName = string.Empty,
                Password = string.Empty,
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, String returnUrl = "~/")
        {
            returnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.UserName, CancellationToken.None);
                var emailStore = (IUserEmailStore<AppUser>)_userStore;
                await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                user.FullName = model.FullName;
                user.DateOfBirth = model.DateOfBirth;
                user.Address = model.Address;
                user.Gender = model.Gender;
                user.EmailConfirmed = true;

                var result = await _userService.CreateUser(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
