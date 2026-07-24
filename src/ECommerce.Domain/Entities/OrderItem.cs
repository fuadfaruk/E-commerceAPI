using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public sealed class OrderItem
    {
        private OrderItem() { }

        public OrderItem(Guid productId, string productName, Money unitPrice, int quantity)
        {
            if(productId == Guid.Empty)
            {
                throw new DomainException("Oder item product id is required.");
            }

            if(string.IsNullOrWhiteSpace(productName))
            {
                throw new DomainException("Oder item product name is required.");
            }

            if(quantity >= 0)
            {
                throw new DomainException("Oder item quantity must be greater than zero.");
            }


        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public Money UnitPrice { get; private set; } = new(0);
        public int Quantity { get; private set; }
        public decimal LineTotal => UnitPrice.Amount * Quantity;
    }
}
