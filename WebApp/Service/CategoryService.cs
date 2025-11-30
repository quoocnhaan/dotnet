using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Service
{
    public class CategoryService
    {
        private readonly AppDBContext _context;

        public CategoryService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                                .Include(c => c.Products)
                                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                                .Include(c => c.Products)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpsertAsync(Category category)
        {
            if (category.Id == 0)
                _context.Categories.Add(category);
            else
                _context.Categories.Update(category);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
