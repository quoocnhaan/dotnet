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

        public IActionResult Index()
        {
            List<Product> products = _context.Products.Include(p => p.Category).ToList();
            SetCartProductCount();
            return View(products);
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

        public IActionResult Create()
        {
            ViewData["Category"] = _context.Categories.ToList();
            SetCartProductCount();

            return View("Upsert", new Product());
        }

        public IActionResult Edit(int id)
        {
            ViewData["Category"] = _context.Categories.ToList();
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            SetCartProductCount();
            return View("Upsert", product);
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            SetCartProductCount();
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
