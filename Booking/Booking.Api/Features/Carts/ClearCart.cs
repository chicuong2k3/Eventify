namespace Booking.Api.Features.Carts;

public record ClearCart(Guid OwnerId) : IRequest;

internal sealed class DeleteCartHandler(ICartRepository cartRepository)
    : IRequestHandler<ClearCart>
{
    public async Task Handle(ClearCart command, CancellationToken cancellationToken)
    {
        await cartRepository.RemoveAsync(command.OwnerId, cancellationToken);
    }
}