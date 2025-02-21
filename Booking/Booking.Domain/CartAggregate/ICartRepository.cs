namespace Booking.Domain.CartAggregate;

public interface ICartRepository
{
    Task<Cart?> GetAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Cart?> UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
