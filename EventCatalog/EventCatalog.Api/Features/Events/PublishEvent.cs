namespace EventCatalog.Api.Features.Events;

public sealed record PublishEvent(Guid EventId) : IRequest;

internal sealed class PublishEventHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PublishEvent>
{
    public async Task Handle(PublishEvent command, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (@event == null)
        {
            throw new NotFoundException($"Event with id {command.EventId} not found");
        }

        @event.Publish();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}