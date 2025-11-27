using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using WebApp.Common.Constants;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {


        public HomeController(UserManager<AppUser> userManager, AppDBContext context)
            : base(userManager, context)
        {
        }


        public IActionResult Index()
        {           
            List<Product> products = _context.Products.ToList();
            SetCartProductCount();
            return View(products);
        }

        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var products = await _context.Products
                             .Where(p => p.CategoryId == categoryId)
                             .Include(p => p.Category)
                             .ToListAsync();

            SetCartProductCount();

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
