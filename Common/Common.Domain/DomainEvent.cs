namespace Common.Domain;

public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; }

    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
    protected DomainEvent(Guid id, DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }
}
