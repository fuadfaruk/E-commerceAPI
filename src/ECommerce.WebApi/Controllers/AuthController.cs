using ECommerce.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers
{
    /// <summary>
    /// Authentication endpoint for user registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Registers a new user and returns a JWT token upon successful registration.
        /// </summary>
        /// <param name="command">Registration details including first name, last name, email, and password.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>Authentication response containing user ID, customer ID, email and JWT token.</returns>
        /// <response code="200">Returns the authentication response with JWT token.</response>
        /// <response code="400">If the registration details are invalid or the email is already in use.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Register(RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Logs in an existing user and returns a JWT token upon successful authentication.
        /// </summary>
        /// <param name="command">Login credentials (email and password).</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>Authentication response containing user ID, customer ID, email, and JWT token.</returns>
        /// <response code="200">Login sucesssful and returns the authentication response with JWT token.</response>
        /// <response code="400">Invaild credentials or login failed.</response>
        /// <response code="401">Unauthorized - invalid email or password.</response>
        /// 
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
