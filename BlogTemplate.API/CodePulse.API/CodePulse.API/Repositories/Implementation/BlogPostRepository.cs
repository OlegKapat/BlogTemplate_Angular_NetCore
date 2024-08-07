using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefault(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _dbContext
                .BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
                await _dbContext.SaveChangesAsync();
                return blogPost;
            }
            return null;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existBlogPost = await _dbContext
                .BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existBlogPost != null)
            {
                _dbContext.BlogPosts.Remove(existBlogPost);
                await _dbContext.SaveChangesAsync();
                return existBlogPost;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
