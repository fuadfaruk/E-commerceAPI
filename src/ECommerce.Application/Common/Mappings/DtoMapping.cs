using ECommerce.Application.Features.Orders;
using ECommerce.Application.Features.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings
{
    public static class DtoMapping
    {
        public static ProductDto ToDto(this Product product) =>
            new(product.Id, product.Name, product.Description, product.Price.Amount, product.Price.Currency, product.StockQuantity);

        public static OrderDto ToDto(this Order order) =>
            new(
                order.Id,
                order.CustomerId,
                order.OrderDate,
                order.Status.ToString(),
                order.Total,
                order.Items.Select(item => new OrderItemDto(
                    item.ProductId,
                    item.ProductName,
                    item.UnitPrice.Amount,
                    item.UnitPrice.Currency,
                    item.Quantity,
                    item.LineTotal)).ToList());
    }
}
