using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly ILogger<BlogPostController> _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostController(
            ILogger<BlogPostController> logger,
            IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository
        )
        {
            _logger = logger;
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            foreach (var categoryId in request.Categories)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category is not null)
                {
                    blogPost.Categories.Add(category);
                }
            }
            await _blogPostRepository.CreateAsync(blogPost);
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost
                    .Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    })
                    .ToList()
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPost = await _blogPostRepository.GetAllAsync();
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var response = blogPost.Select(static bp => new BlogPostDto
            {
                Id = bp.Id,
                Author = bp.Author,
                Content = bp.Content,
                FeaturedImageUrl = bp.FeaturedImageUrl,
                IsVisible = bp.IsVisible,
                PublishedDate = bp.PublishedDate,
                ShortDescription = bp.ShortDescription,
                Title = bp.Title,
                UrlHandle = bp.UrlHandle,
                Categories = bp
                    .Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    })
                    .ToList()
            });
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            return Ok(response);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);
            if (blogPost is null)
            {
                return NotFound();
            }
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost
                    .Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    })
                    .ToList()
            };
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            return Ok(response);
        }

        [HttpPut("{id:Guid}")]
         [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> UpdateBlogPost(
            [FromRoute] Guid id,
            [FromBody] UpdateBlogPostRequestDto request
        )
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            foreach (var categoryId in request.Categories)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category is not null)
                {
                    blogPost.Categories.Add(category);
                }
            }
            var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost is null)
            {
                return NotFound();
            }
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var response = new BlogPostDto
            {
                Id = updatedBlogPost.Id,
                Author = updatedBlogPost.Author,
                Content = updatedBlogPost.Content,
                FeaturedImageUrl = updatedBlogPost.FeaturedImageUrl,
                IsVisible = updatedBlogPost.IsVisible,
                PublishedDate = updatedBlogPost.PublishedDate,
                ShortDescription = updatedBlogPost.ShortDescription,
                Title = updatedBlogPost.Title,
                UrlHandle = updatedBlogPost.UrlHandle,
                Categories = updatedBlogPost
                    .Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    })
                    .ToList()
            };
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

            return Ok(response);
        }

        [HttpDelete("{id:Guid}")]
         [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.DeleteAsync(id);
            if (blogPost is null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
            };
            return Ok(response);
        }
        [HttpGet("{url}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string url)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(url);
            if (blogPost is null)
            {
                return NotFound();
            }
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost
                    .Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    })
                    .ToList()
            };
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            return Ok(response);
        }
    }
}
