using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly Data.AppDBContext _context;
        public CategoryViewComponent(Data.AppDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
