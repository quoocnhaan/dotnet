using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _context;

        public ProductController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
