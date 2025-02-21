using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Booking.Api.Features.Carts;

public class CartRepository : ICartRepository
{
    private readonly IDistributedCache cache;

    public CartRepository(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task RemoveAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(ownerId.ToString());
    }

    public async Task<Cart?> GetAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        var cart = await cache.GetStringAsync(ownerId.ToString(), cancellationToken);

        if (string.IsNullOrEmpty(cart))
        {
            return null;
        }

        return JsonSerializer.Deserialize<Cart>(cart);
    }

    public async Task<Cart?> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await cache.SetStringAsync(cart.OwnerId.ToString(), JsonSerializer.Serialize(cart));
        return await GetAsync(cart.OwnerId);
    }
}
