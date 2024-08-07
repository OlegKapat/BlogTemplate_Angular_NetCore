using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IImageRepository _imageRepository;

        public ImagesController(ILogger<ImagesController> logger, IImageRepository imageRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(
            [FromForm] IFormFile file,
            [FromForm] string fileName,
            [FromForm] string title
        )
        {
            ValidateFileUpload(file);
            if (file is null)
            {
                return BadRequest();
            }
            var image = new BlogImage
            {
                FileName = fileName,
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                Title = title,
                DateCreated = DateTime.Now
            };
            var blogImage = await _imageRepository.UploadImageAsync(file, image);

            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                FileName = blogImage.FileName,
                FileExtension = blogImage.FileExtension,
                Title = blogImage.Title,
                Url = blogImage.Url,
                DateCreated = blogImage.DateCreated
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetImagesAsync();
            var response = images.Select(i => new BlogImageDto
            {
                Id = i.Id,
                FileName = i.FileName,
                FileExtension = i.FileExtension,
                Title = i.Title,
                Url = i.Url,
                DateCreated = i.DateCreated
            });
            return Ok(response);
        }
        

        private void ValidateFileUpload(IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (file.Length == 0)
            {
                throw new ArgumentException("File is empty", nameof(file));
            }
            if (file.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("File is too large", nameof(file));
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("file", "Invalid file extension");
            }
        }
    }
}
