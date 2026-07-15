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

        }
    }
}
