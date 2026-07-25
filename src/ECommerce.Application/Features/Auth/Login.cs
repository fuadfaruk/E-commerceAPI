using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Auth
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
    public sealed class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(160)
                .WithMessage("Email must not exceed 160 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(128)
                .WithMessage("Password must not exceed 128 characters.");
        }
    }

    public sealed class LoginHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, AuthResponse>
    {
        public Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return identityService.LoginAsync(request, cancellationToken);
        }
    }
}

