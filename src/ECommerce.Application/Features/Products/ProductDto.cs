using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a product.
    /// </summary>
    public sealed record ProductDto
    (
        /// <summary>Gets unique identifier of the product.</summary>
        Guid Id,
        /// <summary>Gets product name.</summary>
        string Name,
        /// <summary>Gets product description.</summary>
        string Description,
        /// <summary>Gets product price.</summary>
        decimal Price,
        /// <summary>Gets currency code (ISO 4217).</summary>
        string Currency,
        /// <summary>Gets the quantity of the product in stock.</summary>
        int StockQuantity
    );
}
