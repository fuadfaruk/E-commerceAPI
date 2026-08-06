using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Mappings;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Orders
{
    public sealed record PlaceOrderCommand(AddressDto ShippingAddress, IReadOnlyList<PlaceOrderItemDto> Items) : IRequest<OrderDto>;

    public sealed class PlaceOrderValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderValidator()
        {
            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .WithMessage("Shipping address is required.");

            RuleFor(x => x.ShippingAddress.Street)
                .NotEmpty()
                .WithMessage("Street is required.");

            RuleFor(x => x.ShippingAddress.City)
                .NotEmpty()
                .WithMessage("City is required.");

            RuleFor(x => x.ShippingAddress.Country)
                .NotEmpty()
                .WithMessage("Country is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Order must contain at least one item.")
                .Custom((items, context) =>
                {
                    var duplicates = items
                        .GroupBy(i => i.ProductId)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();

                    if(duplicates.Count > 0)
                    {
                        context.AddFailure("Items", $"Duplicate product IDs found: {string.Join(", ", duplicates)}.");
                    }
                });
            
            RuleForEach(x => x.Items)
                .ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID is required.");
                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.")
                    .LessThanOrEqualTo(1000)
                    .WithMessage("Quantity cannot exceed 1000.");
            });
        }
    }

    public sealed class PlaceOrderHandler(
        ICurrentUserService currentUser,
        ICustomerRepository customers,
        IProductRepository products,
        IOrderRepository orders,
        IApplicationDbContext dbContext) : IRequestHandler<PlaceOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(currentUser.UserId))
            {
                throw new UnauthorizedAccessException("You must be logged in to place an order");
            }

            var customer = await customers.GetByIdentityIdAsyc(currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException("Customer", currentUser.UserId);

            var productIds = request.Items.Select(item => item.ProductId).Distinct().ToList();
            var productMap = (await products.GetByIdsAsync(productIds, cancellationToken)).ToDictionary(p => p.Id, p => p);

            var address = request.ShippingAddress;
            var order = new Order(customer.Id, new Address(address.Street, address.City, address.State, address.PostalCode, address.Country));

            foreach (var item in request.Items)
            {
                if (!productMap.TryGetValue(item.ProductId, out var product))
                {
                    throw new NotFoundException("Product", item.ProductId.ToString());
                }
                order.AddItem(product, item.Quantity);
            }

            if(order.Items.Count == 0)
            {
                throw new DomainException("Order must contain at least one valid item.");
            }

            await orders.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return order.ToDto();
        }
    }
}

