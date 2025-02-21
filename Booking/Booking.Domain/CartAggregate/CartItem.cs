namespace Booking.Domain.CartAggregate;

public class CartItem : Entity
{
    public Guid TicketId { get; }
    public Money Price { get; }
    public int Quantity { get; private set; }

    internal CartItem(Guid ticketId, Money price, int quantity)
    {
        TicketId = ticketId;
        Price = price;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0)
        {
            throw new DomainException("Additional quantity must be greater than zero.");
        }

        Quantity += additionalQuantity;
    }

    public void DecreaseQuantity(int reducedQuantity)
    {
        if (reducedQuantity <= 0)
        {
            throw new DomainException("Reduced quantity must be greater than zero.");
        }

        if (Quantity < reducedQuantity)
        {
            throw new DomainException($"Reduced quantity cannot be greater than {Quantity}.");
        }

        Quantity -= reducedQuantity;
    }
}
