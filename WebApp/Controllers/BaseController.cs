using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly AppDBContext _context;

        public BaseController(UserManager<AppUser> userManager, AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        protected void SetCartProductCount()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser != null)
            {
                int? cartProductCount = _context.Orders
                    .Where(o => o.UserId == currentUser.Id)
                    .SelectMany(o => o.OrderProducts)
                    .Sum(op => op.Quantity);

                TempData["CartProductCount"] = cartProductCount;
            }
        }
    }
}
