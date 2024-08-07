using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ImageRepository(
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context
        )
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<BlogImage> UploadImageAsync(IFormFile file, BlogImage image)
        {
            var uploads = Path.Combine(
                _env.ContentRootPath,
                "Images",
                $"{image.FileName}{image.FileExtension}"
            );
            using (var fileStream = new FileStream(uploads, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var url =
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.Url = url;
            await _context.BlogImages.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<IEnumerable<BlogImage>> GetImagesAsync()
        {
            return await _context.BlogImages.ToListAsync();
        }
    }
}
