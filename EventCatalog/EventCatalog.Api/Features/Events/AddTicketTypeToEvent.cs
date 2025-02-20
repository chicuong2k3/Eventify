namespace EventCatalog.Api.Events;

public sealed record AddTicketTypeToEvent(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
) : IRequest;


internal sealed class CreateTicketTypeCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork,
    ICurrencyLookup currencyLookup)
    : IRequestHandler<AddTicketTypeToEvent>
{
    public async Task Handle(AddTicketTypeToEvent command, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (@event == null)
        {
            throw new NotFoundException($"Event with id {command.EventId} not found");
        }

        @event.AddTicketType(
            command.Name,
            Money.FromDecimal(currencyLookup, command.Price, command.Currency),
            command.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}
