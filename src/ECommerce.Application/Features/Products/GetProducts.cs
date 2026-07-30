using ECommerce.Application.Common.Mappings;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Products
{
    public sealed record class GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>;

    public sealed class GetProductsHandler(IProductRepository products) : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
    {
        public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var list = await products.ListAsync(cancellationToken);
            return list.Select(product => product.ToDto()).ToList();
        }
    }
}

