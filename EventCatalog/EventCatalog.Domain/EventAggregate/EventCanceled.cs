namespace EventCatalog.Domain.EventAggregate;

public sealed record EventCanceled(Guid EventId) : DomainEvent;