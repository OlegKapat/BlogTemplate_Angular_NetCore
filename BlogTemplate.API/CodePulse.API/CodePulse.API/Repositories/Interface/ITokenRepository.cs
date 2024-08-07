using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string GenerateToken(IdentityUser user, IList<string> roles);
    }
}