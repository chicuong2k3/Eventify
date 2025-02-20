namespace EventCatalog.Domain.EventAggregate;

public sealed class TicketType : Entity
{
    private TicketType()
    {
    }

    public string Name { get; private set; }

    public Money Price { get; private set; }

    public int Quantity { get; private set; }

    internal TicketType(
        string name,
        Money price,
        int quantity)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValidationException("Ticket type name is required.");
        if (quantity <= 0)
            throw new ValidationException("Quantity must be greater than zero.");

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Quantity = quantity;

        Raise(new TicketTypeCreated(Id));
    }

    public void UpdatePrice(Money price)
    {
        if (Price != price)
        {
            Price = price;

            Raise(new TicketTypePriceChanged(Id, Price));
        }
    }
}
