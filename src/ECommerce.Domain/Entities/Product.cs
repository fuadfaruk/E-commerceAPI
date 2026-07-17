using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public sealed class Product : Entity
    {
        private Product()
        {
        }

        public Product(string name, string description, Money price, int stockQuantity)
        {

        }

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Money Price { get; private set; } = new(0);
        public int StockQuantity { get; private set; }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Product name cannot be empty.");
            }
            Name = name.Trim();
        }

        public void UpdateDescription(string description)
        {
            Description = description?.Trim() ?? string.Empty;
        }

        public void ChangePrice(Money price)
        {
            if (price.Amount < 0)
            {
                throw new DomainException("Product price cannot be negative.");
            }
            Price = price;
        }

        public void SetStock(int stockQuantity)
        {
            if (stockQuantity < 0)
            {
                throw new DomainException("Product stock cannot be negative.");
            }
            StockQuantity = stockQuantity;
        }

        public void RemoveStock(int quantity)
        {
            if (quantity < 0)
            {
                throw new DomainException("Quantity to remove cannot be negative.");
            }
            if (StockQuantity - quantity < 0)
            {
                throw new DomainException($"{Name} has only {StockQuantity} units in stock");
            }
            StockQuantity -= quantity;
        }
    }
}
