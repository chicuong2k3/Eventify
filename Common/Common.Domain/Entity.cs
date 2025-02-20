namespace Common.Domain;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    private readonly List<IDomainEvent> domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. domainEvents];
    protected Entity()
    {

    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    protected void Raise(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}
