namespace EventCatalog.Domain.EventAggregate;

public sealed record EventCreated(Guid EventId) : DomainEvent;