
namespace EventCatalog.Api.Features.Events;

public record DeleteEvent(Guid EventId) : IRequest;

internal sealed class DeleteEventHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteEvent>
{

    public async Task Handle(DeleteEvent command, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);
        if (@event == null)
        {
            throw new NotFoundException($"Event with id {command.EventId} not found");
        }
        eventRepository.Remove(@event);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}