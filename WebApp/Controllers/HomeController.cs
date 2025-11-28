using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebApp.Common.Constants;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CartService _cartService;

        public HomeController(IDbContextFactory<AppDBContext> context, CartService cartService, UserService userService)
            : base(context, userService)
        {
            _cartService = cartService;
        }


        public async Task<IActionResult> Index()
        {           
            List<Product> products = _context.Products.ToList();
            await _cartService.SetCartProductCount();
            return View(products);
        }

        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var products = await _context.Products
                             .Where(p => p.CategoryId == categoryId)
                             .Include(p => p.Category)
                             .ToListAsync();

            await _cartService.SetCartProductCount();

            return View("Index", products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
