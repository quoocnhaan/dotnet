using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : BaseController
    {

        public ProductController(UserManager<AppUser> userManager, AppDBContext context)
            : base(userManager, context)
        {
        }
        public IActionResult Detail(int id)
        {
            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            SetCartProductCount();
            return View(product);
        }

        private void SetCartProductCount()
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
