namespace Booking.Domain.CartAggregate;

public class Cart : AggregateRoot
{
    public Guid OwnerId { get; }
    public DateTime CreatedAt { get; }
    private List<CartItem> items = new();
    public IReadOnlyCollection<CartItem> Items => items.AsReadOnly();

    public Cart(Guid ownerId)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Guid ticketId, Money price, int quantity)
    {
        // Check whether the ticket exists in the event catalog or not
        // ...

        var existingItem = items.FirstOrDefault(i => i.TicketId == ticketId);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            items.Add(new CartItem(ticketId, price, quantity));
        }
    }

    public void RemoveItem(Guid ticketId, int quantity)
    {
        var existingItem = items.FirstOrDefault(i => i.TicketId == ticketId);

        if (existingItem != null)
        {
            existingItem.DecreaseQuantity(quantity);
            if (existingItem.Quantity == 0)
            {
                items.Remove(existingItem);
            }
        }
    }
}
