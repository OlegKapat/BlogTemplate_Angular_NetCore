using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> UploadImageAsync(IFormFile file,BlogImage image);
        Task<IEnumerable<BlogImage>> GetImagesAsync();
    }
}