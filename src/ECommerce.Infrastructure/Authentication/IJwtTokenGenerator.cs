using ECommerce.Infrastructure.Identity;

namespace ECommerce.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
