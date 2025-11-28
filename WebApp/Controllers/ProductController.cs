using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly CartService _cartService;

        public ProductController(IDbContextFactory<AppDBContext> context, CartService cartService, UserService userService)
            : base(context, userService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = _context.Products.Include(p => p.Category).ToList();
            await _cartService.SetCartProductCount();
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            await _cartService.SetCartProductCount();
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Category"] = _context.Categories.ToList();
            await _cartService.SetCartProductCount();

            return View("Upsert", new Product());
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Category"] = _context.Categories.ToList();
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            await _cartService.SetCartProductCount();
            return View("Upsert", product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products.Find(id);
            await _cartService.SetCartProductCount();
            return View(product);
        }

        [HttpPost]
        public IActionResult Upsert(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
            {
                _context.Products.Add(model);
            }
            else
            {
                _context.Products.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
