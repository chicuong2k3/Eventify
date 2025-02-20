namespace EventCatalog.Domain.EventAggregate;

public sealed record TicketTypeAddedToEvent(Guid EventId, Guid TicketTypeId) : DomainEvent;