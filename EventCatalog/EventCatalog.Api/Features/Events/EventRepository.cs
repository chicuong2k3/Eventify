using EventCatalog.Api.Persistence;

namespace EventCatalog.Api.Features.Events;

internal sealed class EventRepository(EventCatalogDbContext dbContext) : IEventRepository
{
    public void Add(Event @event)
    {
        dbContext.Events.Add(@event);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Events.Where(e => e.Id == id)
                    .Include(e => e.TicketTypes)
                    .FirstOrDefaultAsync(cancellationToken);
    }

    public void Remove(Event @event)
    {
        dbContext.Events.Remove(@event);
    }

    public async Task<PaginationResult<Event>> SearchEventsAsync(EventSpecification specification, CancellationToken cancellationToken = default)
    {
        var events = dbContext.Events.AsQueryable();
        if (!string.IsNullOrWhiteSpace(specification.SearchText))
        {
            events = events.Where(e => e.Title.ToLower().Contains(specification.SearchText.ToLower()));
        }

        if (specification.CategoryId != null)
        {
            events = events.Where(e => e.CategoryId == specification.CategoryId);
        }

        if (specification.Schedule != null)
        {
            events = events.Where(e => e.Schedule.Start >= specification.Schedule.Start);

            if (specification.Schedule.End != null)
            {
                events = events.Where(e => e.Schedule.End == null || e.Schedule.End < specification.Schedule.End);
            }
        }

        if (specification.MaxCapacity != null)
        {
            events = events.Where(e => e.Capacity == null || e.Capacity < specification.MaxCapacity);
        }

        var totalRecords = await events.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(specification.SortBy))
        {
            events = events.OrderBy(e => e.Title);
        }
        else
        {
            var rules = specification.SortBy.Split(',')
                                     .Select(rule => rule.Trim())
                                     .Where(rule => !string.IsNullOrWhiteSpace(rule))
                                     .ToList();

            foreach (var rule in rules)
            {
                var components = rule.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (components.Length != 2)
                {
                    throw new ValidationException($"Invalid sorting rule: '{rule}'. Expected format: 'column direction'");
                }

                var sortColumn = components[0].ToLower();
                var sortDirection = components[1].ToLower();

                events = ApplySorting(events, sortColumn, sortDirection);
            }
        }

        var pagedEvents = await events
                                .Skip((specification.PageNumber - 1) * specification.PageSize)
                                .Take(specification.PageSize)
                                .ToListAsync(cancellationToken);


        return new PaginationResult<Event>(specification.PageNumber, specification.PageSize, totalRecords, pagedEvents);

    }

    private static IQueryable<Event> ApplySorting(IQueryable<Event> query, string sortColumn, string sortDirection)
    {
        return sortDirection switch
        {
            "asc" => sortColumn switch
            {
                "title" => query.OrderBy(e => e.Title),
                "capacity" => query.OrderBy(e => e.Capacity),
                "schedule" => query.OrderBy(e => e.Schedule.Start),
                _ => throw new ValidationException($"Unsupported sort column: '{sortColumn}'")
            },
            "desc" => sortColumn switch
            {
                "title" => query.OrderByDescending(e => e.Title),
                "capacity" => query.OrderByDescending(e => e.Capacity),
                "schedule" => query.OrderByDescending(e => e.Schedule.Start),
                _ => throw new ValidationException($"Unsupported sort column: '{sortColumn}'")
            },
            _ => throw new ValidationException($"Unsupported sort direction: '{sortDirection}'. Sort direction must be 'asc' or 'desc'")
        };
    }
}
