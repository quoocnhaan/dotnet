using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CartService _cartService;

        public CategoryController(IDbContextFactory<AppDBContext> context, CartService cartService, UserService userService)
    : base(context, userService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = _context.Categories.Include(c => c.Products).ToList();
            await _cartService.SetCartProductCount();

            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            await _cartService.SetCartProductCount();

            return View("Upsert", new Category());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            await _cartService.SetCartProductCount();
            return View("Upsert", category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            await _cartService.SetCartProductCount();
            return View(category);
        }

        [HttpPost]
        public IActionResult Upsert(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
            {
                _context.Categories.Add(model);
            }
            else
            {
                _context.Categories.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]    
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
