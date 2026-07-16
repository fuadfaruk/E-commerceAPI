using ECommerce.Domain.Common;
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


    }
}
