using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(UserManager<AppUser> userManager, AppDBContext context)
    : base(userManager, context)
        {
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.Include(c => c.Products).ToList();
            SetCartProductCount();

            return View(categories);
        }

        public IActionResult Create()
        {
            SetCartProductCount();

            return View("Upsert", new Category());
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            SetCartProductCount();
            return View("Upsert", category);
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            SetCartProductCount();
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
