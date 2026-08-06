using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Mappings;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Orders
{
    public sealed record GetMyOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;

    public sealed class GetMyOrdersHandler(
        ICurrentUserService currentUsers,
        ICustomerRepository customers,
        IOrderRepository orders) : IRequestHandler<GetMyOrdersQuery, IReadOnlyList<OrderDto>>
    {
        public async Task<IReadOnlyList<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(currentUsers.UserId))
            {
                throw new UnauthorizedAccessException("You must be logged in to view orders.");
            }

            var customer = await customers.GetByIdentityIdAsyc(currentUsers.UserId, cancellationToken)
                ?? throw new NotFoundException("Customer", currentUsers.UserId);

            var list = await orders.GetOrdersForCustomerAsync(customer.Id, cancellationToken);
            return list.Select(order => order.ToDto()).ToList();
        }
    }
}

