using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Mappings;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    public sealed record class CreateProductCommand(string Name, string Description, decimal Price, string Currency, int StockQuantity) : IRequest<ProductDto>;
    public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(120)
                .WithMessage("Product name must not exceed 120 characters.")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters long.");
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Product description must not exceed 1000 characters.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product price must be a non-negative value.")
                .LessThanOrEqualTo(999999.99m)
                .WithMessage("Product price is too high.");
            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Product currency is required.")
                .Length(3)
                .WithMessage("Product currency must be a 3-letter ISO currency code.")
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency must be uppercase 3-letter ISO 4217 currency code.");
            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative.")
                .LessThanOrEqualTo(999999)
                .WithMessage("Stock quantity is too high.");
        }
    }

    public sealed class CreateProductHandler(IProductRepository products, IApplicationDbContext dbContext) 
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.Name, request.Description, new Money(request.Price, request.Currency), request.StockQuantity);

            await products.AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return product.ToDto();
        }
    }
}
