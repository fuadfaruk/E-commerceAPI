using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found.
    /// </summary>
    public sealed class NotFoundException(string name, object key)
        : Exception($"{name} with key '{key}' was not found.");

    /// <summary>
    /// Exception thrown when a user attempts to access a resource they do not have permission to access.
    /// </summary>
    public sealed class ForbiddenAcessException(string message)
        : Exception(message);

    /// <summary>
    /// Exception thrown when a conflict occurs, such as when trying to create a resource that already exists.
    /// </summary>
    public sealed class ConflictException(string message)
        : Exception(message);
}
