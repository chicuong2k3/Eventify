namespace EventCatalog.Domain.CategoryAggregate;

public sealed record CategoryArchived(Guid CategoryId) : DomainEvent;