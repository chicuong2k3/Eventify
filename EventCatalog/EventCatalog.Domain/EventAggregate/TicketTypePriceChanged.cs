namespace EventCatalog.Domain.EventAggregate;

public sealed record TicketTypePriceChanged(Guid TicketTypeId, Money Price) : DomainEvent;
