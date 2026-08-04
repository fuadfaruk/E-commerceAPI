using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Products
{
    public sealed record DeleteProductCommand(Guid Id) : IRequest;

    public sealed class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
        }
    }

    public sealed class DeleteProductHandler(IProductRepository products, IApplicationDbContext dbContext) 
        : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await products.GetByIdAsync(request.Id, cancellationToken)
                ??  throw new NotFoundException("Product", request.Id);

            products.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

