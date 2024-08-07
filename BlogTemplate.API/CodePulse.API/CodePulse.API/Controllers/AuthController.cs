using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, "Reader");
                if (result.Succeeded)
                {
                    return Ok();
                }
                else if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            else
            {
                if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (result)
                {
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = (List<string>)await _userManager.GetRolesAsync(user),
                        Token = _tokenRepository.GenerateToken(user, (List<string>)await _userManager.GetRolesAsync(user))
                    };
                  return Ok(response);
                }
              
            }
            ModelState.AddModelError("Unauthorized", "Invalid email or password");
            return Unauthorized();
        }
    }
}
