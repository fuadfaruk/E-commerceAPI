using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ECommerce.Domain.ValueObjects
{
    public sealed record Money
    {
        private Money()
        {
        }

        public Money(decimal amount, string currency = "USD")
        {
            if (amount < 0)
            {
                throw new DomainException("Amount cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            {
                throw new DomainException("Currency is required.");
            }
            Amount = amount;
            Currency = currency.Trim().ToUpperInvariant();
        }

        public decimal Amount { get; private init; }
        public string Currency { get; private init; } = "USD";
    }
}
