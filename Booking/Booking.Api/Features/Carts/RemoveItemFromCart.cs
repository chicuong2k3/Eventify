namespace Booking.Api.Features.Carts;

public record RemoveItemFromCart(Guid OwnerId, Guid TicketId, int Quantity) : IRequest<Cart>;

internal sealed class RemoveItemFromCartHandler(
    ICartRepository cartRepository)
    : IRequestHandler<RemoveItemFromCart, Cart>
{
    public async Task<Cart> Handle(RemoveItemFromCart command, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetAsync(command.OwnerId, cancellationToken);
        if (cart == null)
        {
            cart = new Cart(command.OwnerId);
        }

        cart.RemoveItem(command.TicketId, command.Quantity);
        await cartRepository.UpdateAsync(cart, cancellationToken);
        return cart;
    }
}