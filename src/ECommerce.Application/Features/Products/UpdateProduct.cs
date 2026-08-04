using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    public sealed record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, string Currency, int StockQuantity) : IRequest<ProductDto>;

    public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(120)
                .WithMessage("Product name cannot exceed 120 characters.")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters long.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Product description cannot exceed 1000 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product price must be a non-negative value.")
                .LessThanOrEqualTo(999999.99m)
                .WithMessage("Product price is too high.");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency is required.")
                .Length(3)
                .WithMessage("Currency must be a 3-letter ISO currency code.")
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency must be uppercase 3-letter ISO 4217 currency code.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative.")
                .LessThanOrEqualTo(999999)
                .WithMessage("Stock quantity is too high.");
        }
    }
}
