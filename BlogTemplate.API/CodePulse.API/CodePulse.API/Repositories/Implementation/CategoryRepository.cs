using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?>? DeleteAsync(Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return category;
            }

            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(
            string query,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 5
        )
        {
            var categories = _context.Categories.AsQueryable();
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                categories = categories.Where(c => c.Name.Contains(query));
            }
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase))
                    {
                        categories = categories.OrderBy(c => c.Name);
                    }
                    else if (
                        string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        categories = categories.OrderByDescending(c => c.Name);
                    }
                }

                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase))
                    {
                        categories = categories.OrderBy(c => c.UrlHandle);
                    }
                    else if (
                        string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        categories = categories.OrderByDescending(c => c.UrlHandle);
                    }
                }
                var skipResults = (pageNumber - 1) * pageSize;
                categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 5);
            }

            return await categories.ToListAsync();
        }

        public Task<Category?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.Categories.FirstOrDefault(c => c.Id == id));
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c =>
                c.Id == category.Id
            );
            if (existingCategory != null)
            {
                _context.Entry(category).CurrentValues.SetValues(category);
                await _context.SaveChangesAsync();
                return category;
            }

            return null;
        }
    }
}
