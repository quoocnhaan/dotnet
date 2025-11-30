using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CategoryService _categoryService;

        public CategoryController(
            IDbContextFactory<AppDBContext> contextFactory,
            CartService cartService,
            UserService userService,
            CategoryService categoryService
        ) : base(contextFactory, userService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return View("Upsert", category);
        }

        public IActionResult Create()
        {
            return View("Upsert", new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.UpsertAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _categoryService.DeleteAsync(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
