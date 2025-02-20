namespace EventCatalog.Api.Events;

public record SearchEvent(
    int PageSize,
    int PageNumber,
    Guid? CategoryId,
    string? SearchText,
    int? MaxCapacity,
    DateTime? StartsAt,
    DateTime? EndsAt,
    string? SortBy) : IRequest<PaginationResult<Event>>;

internal sealed class SearchEventsQueryHandler(
    IEventRepository eventRepository)
    : IRequestHandler<SearchEvent, PaginationResult<Event>>
{
    public async Task<PaginationResult<Event>> Handle(SearchEvent query, CancellationToken cancellationToken)
    {
        var schedule = query.StartsAt == null ? null
            : new DateTimeRange(query.StartsAt.Value, query.EndsAt);

        var specification = new EventSpecification(
            query.PageSize,
            query.PageNumber,
            query.CategoryId,
            query.SearchText,
            query.MaxCapacity,
            schedule,
            query.SortBy);

        return await eventRepository.SearchEventsAsync(specification, cancellationToken);
    }
}
