namespace EventCatalog.Domain.EventAggregate;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Event @event);
    void Remove(Event @event);
    Task<PaginationResult<Event>> SearchEventsAsync(EventSpecification specification, CancellationToken cancellationToken = default);
}
