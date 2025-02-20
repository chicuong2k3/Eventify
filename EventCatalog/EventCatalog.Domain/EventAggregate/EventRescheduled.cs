namespace EventCatalog.Domain.EventAggregate;

public sealed record EventRescheduled(Guid EventId, DateTimeRange Schedule) : DomainEvent;