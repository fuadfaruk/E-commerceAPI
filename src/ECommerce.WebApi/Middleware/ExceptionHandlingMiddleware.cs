using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions and converting them to standardized RFC 7807 Problem Details responses.
    /// </summary>
    public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        /// <summary>
        /// Processes the HTTP request and catches any unhandled exceptions.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred: {ExceptionMessage}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception and writes appropriate response.
        /// </summary>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var problem = exception switch
            {
                ValidationException validationException => BuildValidationProblem(context, validationException),
                DomainException domainException => BuildProblem(context, StatusCodes.Status400BadRequest, "Business rule violation", domainException.Message),
                NotFoundException notFoundException => BuildProblem(context, StatusCodes.Status404NotFound, "Resource not found", notFoundException.Message),
                UnauthorizedAccessException unauthorizedException => BuildProblem(context, StatusCodes.Status401Unauthorized, "Unauthorized", unauthorizedException.Message),
                InvalidOperationException invalidException => BuildProblem(context, StatusCodes.Status400BadRequest, "Invalid operation", invalidException.Message),
                _ => BuildProblem(context, StatusCodes.Status500InternalServerError, "Internal server error", "An unexpected error occurred.")
            };

            context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problem);
        }

        /// <summary>
        /// Builds a standard problem details response.
        /// </summary>
        private static ProblemDetails BuildProblem(HttpContext context, int statusCode, string title, string detail)
        {
            return new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path,
                Type = $"https://httpwg.org/specs/rfc7231.html#{statusCode}"
            };
        }

        /// <summary>
        /// Builds a problem details response for validation errors.
        /// </summary>
        private static ProblemDetails BuildValidationProblem(HttpContext context, ValidationException validationException)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = "One or more validation errors occurred.",
                Instance = context.Request.Path,
                Type = $"https://httpwg.org/specs/rfc7231.html#400"
            };

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(e => e.ErrorMessage).ToArray(),
                    StringComparer.OrdinalIgnoreCase
                );

            problem.Extensions["errors"] = errors;
            return problem;
        }
    }
}
