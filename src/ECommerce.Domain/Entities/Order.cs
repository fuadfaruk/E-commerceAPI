using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public sealed class Order : Entity
    {
        private readonly List<OrderItem> _items = [];

        private Order() { }

        public Order(Guid customerId, Address shippingAddress)
        {
            if(CustomerId == Guid.Empty)
            {
                throw new DomainException("Order customer id is required.");
            }

            CustomerId = customerId;
            ShippingAddress = shippingAddress;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
        }

        public Guid CustomerId { get; private set; }
        public DateTimeOffset OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public Address ShippingAddress { get; private set; } = new("Unknown", "Unknown", string.Empty, string.Empty, "Unknown");
        public ReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public decimal Total => _items.Sum(i => i.LineTotal);

        public void AddItem(Product product, int quantity)
        {
            product.RemoveStock(quantity);
            _items.Add(new OrderItem(product.Id, product.Name, product.Price, quantity));
        }

        public void MarkPaid()
        {
            if(Status != OrderStatus.Pending)
            {
                throw new DomainException("Only pending orders can be marked as paid.");
            }
            Status = OrderStatus.Paid;
        }
    }
}
