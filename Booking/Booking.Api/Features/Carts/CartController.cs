using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Features.Carts;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{apiVersion}/[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator mediator;

    public CartController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var ownerId = new Guid("F4925C17-1DF2-4293-983D-6E49847745CE");
        var cart = await mediator.Send(new GetCart(ownerId));
        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartRequest request)
    {
        var ownerId = new Guid("F4925C17-1DF2-4293-983D-6E49847745CE");
        var cart = await mediator.Send(new AddItemToCart(ownerId, request.Items));
        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItemFromCart([FromBody] RemoveItemFromCartRequest request)
    {
        var ownerId = new Guid("F4925C17-1DF2-4293-983D-6E49847745CE");
        var cart = await mediator.Send(new RemoveItemFromCart(ownerId, request.TicketId, request.Quantity));
        return Ok(cart);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var ownerId = new Guid("F4925C17-1DF2-4293-983D-6E49847745CE");
        await mediator.Send(new ClearCart(ownerId));
        return NoContent();
    }
}

public record AddItemToCartRequest(List<AddItemRequest> Items) : IRequest<Cart>;
public record RemoveItemFromCartRequest(Guid TicketId, int Quantity) : IRequest<Cart>;