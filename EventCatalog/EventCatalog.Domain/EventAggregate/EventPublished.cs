namespace EventCatalog.Domain.EventAggregate;
public sealed record EventPublished(Guid EventId) : DomainEvent;
