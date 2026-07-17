using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public sealed class Customer : Entity
    {
        private Customer()
        {
        }

        public Customer(string firstName, string lastName, string email, string userIdentityId)
        {
            Rename(firstName, lastName);
            ChangeEmail(email);
            UserIdentityId = string.IsNullOrWhiteSpace(userIdentityId) 
                ? throw new DomainException("User identity ID cannot be empty.") 
                : userIdentityId;
        }

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string UserIdentityId { get; private set; } = string.Empty;

        public void Rename(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException("First name and last name cannot be empty.");
            }
        }

        public void ChangeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                throw new DomainException("Email is invalid.");
            }

            Email = email.Trim().ToLowerInvariant();
        }
    }
}
