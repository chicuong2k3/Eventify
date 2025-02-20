namespace EventCatalog.Api.Features.Events;

public sealed record GetEventById(Guid Id) : IRequest<Event>;

internal sealed class GetEventByIdHandler(
    IEventRepository eventRepository)
    : IRequestHandler<GetEventById, Event>
{
    public async Task<Event> Handle(GetEventById query, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(query.Id, cancellationToken);

        if (@event == null)
        {
            throw new NotFoundException($"Event with id {query.Id} not found.");
        }

        return @event;
    }
}