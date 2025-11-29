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
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.Include(c => c.Products).ToList();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View("Upsert", new Category());
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return View("Upsert", category);
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
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
