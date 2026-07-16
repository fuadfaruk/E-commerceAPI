using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.ValueObjects
{
    public sealed record Address
    {
        private Address()
        {
        }

        public Address(string street, string city, string state, string postalCode, string country)
        {
            if(string.IsNullOrWhiteSpace(street) ||
                string.IsNullOrWhiteSpace(city) ||
                string.IsNullOrWhiteSpace(country))
            {
                throw new DomainException("Street, city, and country are required for an address.");
            }

            Street = street.Trim();
            City = city.Trim();
            State = state.Trim();
            PostalCode = postalCode.Trim();
            Country = country.Trim();
        }

        public string Street { get; private init; } = string.Empty;
        public string City { get; private init; } = string.Empty;
        public string State { get; private init; } = string.Empty;
        public string PostalCode { get; private init; } = string.Empty;
        public string Country { get; private init; } = string.Empty;
    }
}
