using ECommerce.Application.Features.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers
{
    /// <summary>
    /// Product management endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProductsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves all available products.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>List of all products with their details.</returns>
        /// <response code="200">Products retrieved successfully.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(CancellationToken cancellationToken) =>
            Ok(await mediator.Send(new GetProductsQuery(), cancellationToken));

        /// <summary>
        /// Retrieves a specific product by its unique identifier.
        /// </summary>
        /// <param name="id">The product ID (GUID).</param>
        /// <param name="cancellationToken">Cancellation token for the operation</param>
        /// <returns>Product details including name, description, price, and stock quantity.</returns>
        /// <response code="200">Product retrieved successfully.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id, CancellationToken cancellationToken) =>
            Ok(await mediator.Send(new GetProductByIdQuery(id), cancellationToken));

        /// <summary>
        /// Creates a new product with the provided details. Requires authentication and authorization.
        /// </summary>
        /// <param name="command">Product creation details (name, description, price, currency, stock).</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="201">Product created successfully.</response>
        /// <response code="400">Invalid input or business rule violation.</response>
        /// <response code="401">Unauthorized - authentication required.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProductDto>> Create(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var create = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetProduct), new { id = create.Id }, create);
        }

        /// <summary>
        /// Updates an existing product with the provided details. Requires authentication and authorization.
        /// </summary>
        /// <param name="id">The product ID (GUID).</param>
        /// <param name="command">Update product details.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The updated product.</returns>
        /// <response code="200">Product updated successfully.</response>
        /// <response code="400">Invalid input or ID mismatch.</response>
        /// <response code="401">Unauthorized - authentication required.</response>
        /// <response code="404">Product not found.</response>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Update(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "ID Mismatch",
                    Detail = "The ID in the URL does not match the ID in the request body.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            var updatedProduct = await mediator.Send(command, cancellationToken);
            return Ok(updatedProduct);
        }

        /// <summary>
        /// Deletes a specific product by its unique identifier. Requires authentication and authorization.
        /// </summary>
        /// <param name="id">The product ID (GUID).</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Product deleted successfully.</response>
        /// <response code="401">Unauthorized - authentication required.</response>
        /// <response code="404">Product not found.</response>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
