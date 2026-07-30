using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Mappings;
using ECommerce.Domain.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    public sealed record class GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

    public sealed class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
        }
    }

    public sealed class GetProductByIdHandler(IProductRepository products) : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await products.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Product", request.Id);
            return product.ToDto();
        }
    }
}
