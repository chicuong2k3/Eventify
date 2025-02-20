namespace EventCatalog.Domain.CategoryAggregate;

public sealed record CategoryCreated(Guid CategoryId) : DomainEvent;