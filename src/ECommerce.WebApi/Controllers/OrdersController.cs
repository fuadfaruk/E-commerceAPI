using ECommerce.Application.Features.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        [HttpGet("mine")]
        [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMine(CancellationToken cancellationToken) =>
            Ok(await mediator.Send(new GetMyOrdersQuery(), cancellationToken));

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> PlaceOrder(PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetMine), new { id = order.Id }, order);
        }

    }
}
