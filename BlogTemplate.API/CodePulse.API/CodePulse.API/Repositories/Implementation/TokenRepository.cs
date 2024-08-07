using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IdentityUser user, IList<string> roles)
        {
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
