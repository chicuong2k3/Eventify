namespace EventCatalog.Api.Features.Events;

public sealed record CancelEvent(Guid EventId) : IRequest;

internal sealed class CancelEventHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelEvent>
{
    public async Task Handle(CancelEvent command, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (@event == null)
        {
            throw new NotFoundException($"Event with id {command.EventId} not found");
        }

        @event.Cancel();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
