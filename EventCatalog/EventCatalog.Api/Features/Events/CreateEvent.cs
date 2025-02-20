namespace EventCatalog.Api.Features.Events;

public sealed record CreateEvent(
    string Title,
    string Description,
    string Location,
    Guid CategoryId,
    string Ward,
    string District,
    string Province,
    string Country,
    string? Street,
    DateTime StartsAt,
    DateTime? EndsAt,
    int? Capacity
) : IRequest<Event>;


internal sealed class CreateEventHandler(
    IEventRepository eventRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateEvent, Event>
{
    public async Task<Event> Handle(CreateEvent command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException("Category not found");
        }

        var location = new Location(
            command.Street,
            command.Ward,
            command.District,
            command.Province,
            command.Country
        );

        var schedule = new DateTimeRange(command.StartsAt, command.EndsAt);

        var @event = new Event(
            command.Title,
            command.Description,
            schedule,
            location,
            command.CategoryId,
            command.Capacity
        );

        eventRepository.Add(@event);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return @event;
    }

}
