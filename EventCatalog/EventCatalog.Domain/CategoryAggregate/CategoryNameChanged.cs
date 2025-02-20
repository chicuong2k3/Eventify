namespace EventCatalog.Domain.CategoryAggregate;

public sealed record CategoryNameChanged(Guid CategoryId, string CategoryName)
    : DomainEvent;
