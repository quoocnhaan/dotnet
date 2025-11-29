using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebApp.Data;

namespace WebApp.Service
{
    public class UserService
    {
        protected readonly UserManager<AppUser> _userManager;
        protected  readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppUser?> GetUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;

            return await _userManager.GetUserAsync(user);
        }

        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public bool isLogin()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}
