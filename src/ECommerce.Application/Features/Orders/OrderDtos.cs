using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders
{
    /// <summary>
    /// Data Transfer Object (DTO) for shipping address information.
    /// </summary>

    public sealed record AddressDto(
        /// <summary>Gets the street address.</summary>
        string Street,
        /// <summary>Gets the city.</summary>
        string City,
        /// <summary>Gets the state or province.</summary>
        string State,
        /// <summary>Gets the postal code.</summary>
        string PostalCode,
        /// <summary>Gets the country name.</summary>
        string Country
    );

    /// <summary>
    /// Data Transfer Object (DTO) for a product in an order request.
    /// </summary>
    public sealed record PlaceOrderItemDto(
        /// <summary>Gets the product ID.</summary>
        Guid ProductId,
        /// <summary>Gets the quantity of the product to order.</summary>
        int Quantity
    );

    /// <summary>
    /// Data Transfer Object (DTO) for a product item in an order response.
    /// </summary>
    public sealed record OrderItemDto(
        /// <summary>Gets the product ID.</summary>
        Guid ProductId,
        /// <summary>Gets the product name at the time of order.</summary>
        string ProductName,
        /// <summary>Gets the unit price of the product at the time of order.</summary>
        decimal UnitPrice,
        /// <summary>Gets the currency code.</summary>
        string Currency,
        /// <summary>Gets the quantity ordered.</summary>
        int Quantity,
        /// <summary>Gets the total price for this line item (UnitPrice * Quantity).</summary>
        decimal LineTotal
    );

    /// <summary>
    /// Data Transfer Object (DTO) representing a complete order.
    /// </summary>
    public sealed record OrderDto(
        /// <summary>Gets the unique order identifier.</summary>
        Guid Id,
        /// <summary>Gets the customer ID who placed the order.</summary>
        Guid CustomerId,
        /// <summary>Gets the date and time when the order was placed.</summary>
        DateTimeOffset OrderDate,
        /// <summary>Gets the order status (e.g., Pending, Paid, Shipped, Cancelled).</summary>
        string Status,
        /// <summary>Gets the total amount for the order.</summary>
        decimal Total,
        /// <summary>Gets the list of items in the order</summary>
        IEnumerable<OrderItemDto> Items
    );
}
