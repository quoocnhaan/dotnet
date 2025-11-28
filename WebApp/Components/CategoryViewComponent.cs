using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly IDbContextFactory<AppDBContext> _contextFactory;

        public CategoryViewComponent(IDbContextFactory<AppDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            using var context = _contextFactory.CreateDbContext();

            var categories = await context.Categories.ToListAsync();

            return View(categories);
        }
    }
}
