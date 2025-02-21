namespace Booking.Api.Features.Carts;

public record AddItemToCart(Guid OwnerId, List<AddItemRequest> Items) : IRequest<Cart>;

public record AddItemRequest(Guid TicketId, decimal Price, string Currency, int Quantity);

internal sealed class AddItemToCartHandler(
    ICartRepository cartRepository,
    ICurrencyLookup currencyLookup)
    : IRequestHandler<AddItemToCart, Cart>
{
    public async Task<Cart> Handle(AddItemToCart command, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetAsync(command.OwnerId, cancellationToken);

        if (cart == null)
        {
            cart = new Cart(command.OwnerId);
        }

        foreach (var item in command.Items)
        {
            cart.AddItem(item.TicketId, Money.FromDecimal(currencyLookup, item.Price, item.Currency), item.Quantity);
        }

        await cartRepository.UpdateAsync(cart, cancellationToken);
        return cart;
    }
}
