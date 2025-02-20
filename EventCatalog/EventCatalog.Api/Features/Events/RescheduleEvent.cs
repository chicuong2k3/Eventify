namespace EventCatalog.Api.Features.Events;

public sealed record RescheduleEvent(
    Guid EventId,
    DateTime StartsAt,
    DateTime? EndsAt
) : IRequest;


internal sealed class RescheduleEventHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RescheduleEvent>
{
    public async Task Handle(RescheduleEvent command, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (@event == null)
        {
            throw new NotFoundException($"Event with id {command.EventId} not found.");
        }

        @event.Reschedule(new DateTimeRange(command.StartsAt, command.EndsAt));
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
