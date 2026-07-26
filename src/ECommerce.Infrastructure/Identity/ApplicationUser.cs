using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
    }
}

