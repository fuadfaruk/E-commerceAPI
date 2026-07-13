using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Common
{
    abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
