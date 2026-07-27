using ECommerce.Application.Common.Interfaces;
using System.Security.Claims;

namespace ECommerce.WebApi.Services
{
    public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public string? UserId => 
            httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
