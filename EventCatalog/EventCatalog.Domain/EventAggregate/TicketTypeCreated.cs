namespace EventCatalog.Domain.EventAggregate;

public record TicketTypeCreated(Guid TicketTypeId) : DomainEvent;