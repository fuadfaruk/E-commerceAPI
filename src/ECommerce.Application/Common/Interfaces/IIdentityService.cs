using ECommerce.Application.Features.Auth;

namespace ECommerce.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResponse> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default);
        Task<AuthResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
    }
}
