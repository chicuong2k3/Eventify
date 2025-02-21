namespace Booking.Api.Features.Carts;

public record GetCart(Guid OwnerId) : IRequest<Cart>;

internal sealed class GetCartHandler(ICartRepository cartRepository)
    : IRequestHandler<GetCart, Cart>
{
    public async Task<Cart> Handle(GetCart query, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetAsync(query.OwnerId);

        if (cart == null)
        {
            cart = new Cart(query.OwnerId);
        }

        return cart;
    }
}
