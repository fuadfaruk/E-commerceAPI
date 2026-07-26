using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Auth;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Identity
{
    public sealed class IdentityService(
        UserManager<ApplicationUser> userManager,
        ICustomerRepository customers,
        IApplicationDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator) : IIdentityService
    {
        public async Task<AuthResponse> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default)
        {
            var user = new ApplicationUser
            {
                UserName = command.Email,
                Email = command.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var customer = new Customer(command.FirstName, command.LastName, command.Email, user.Id);
            await customers.AddAsync(customer, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var token = jwtTokenGenerator.GenerateToken(user, await userManager.GetRolesAsync(user));
            return new AuthResponse(user.Id, customer.Id, command.Email, token);
        }

        public async Task<AuthResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(command.Email)
                ?? throw new UnauthorizedAccessException("Invalid email or password.");

            if(!await userManager.CheckPasswordAsync(user, command.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var customer = await customers.GetByIdentityIdAsyc(user.Id, cancellationToken)
                ?? throw new InvalidOperationException("Identity user is missing its linked customer.");

            var token = jwtTokenGenerator.GenerateToken(user, await userManager.GetRolesAsync(user));
            return new AuthResponse(user.Id, customer.Id, command.Email, token);
        }
    }
}
