using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _category;

        public CategoriesController(ICategoryRepository category)
        {
            _category = category;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category { Name = request.Name, UrlHandle = request.UrlHandle };
            await _category.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize
        )
        {
            var categories = await _category.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);
            var response = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                UrlHandle = c.UrlHandle
            });
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _category.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory(
            [FromRoute] Guid id,
            UpdateCategoryRequestDto request
        )
        {
            var category = await _category.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            category.Name = request.Name;
            category.UrlHandle = request.UrlHandle;
            category = await _category.UpdateAsync(category);
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _category.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            await _category.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCategoryCount()
        {
            var categories = await _category.GetAllAsync();
            return Ok(categories.Count());
        }
    }
}
