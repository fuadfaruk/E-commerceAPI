namespace ECommerce.Application.Features.Auth
{
    /// <summary>
    /// Auth respose containg user credentials and JWT token for authentication.
    /// </summary>
    public sealed record AuthResponse
    (
        /// <summary>Gets identity user ID from ASP.NET Core Identity.</summary>
        string UserId,
        /// <summary>Gets associated customer ID.</summary>
        Guid CustomerId,
        /// <summary>Gets user email address.</summary>
        string Email,
        /// <summary>Gets JWT authentication token for subsequent API calls</summary>
        string Token
    );
}

