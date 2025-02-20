namespace EventCatalog.Domain.EventAggregate;

public sealed class Event : AggregateRoot
{
    private Event()
    {

    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTimeRange Schedule { get; private set; }
    public Location Location { get; private set; }
    public EventStatus Status { get; private set; }
    public int? Capacity { get; private set; }
    public Guid CategoryId { get; private set; }

    private readonly List<TicketType> ticketTypes = new();
    public IReadOnlyCollection<TicketType> TicketTypes => ticketTypes.AsReadOnly();

    public Event(string title,
        string description,
        DateTimeRange schedule,
        Location location,
        Guid categoryId,
        int? capacity)
    {
        if (string.IsNullOrEmpty(title))
            throw new ValidationException("Title is required.");
        if (schedule == null)
            throw new ValidationException("Schedule is required.");
        if (location == null)
            throw new ValidationException("Location is required.");

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Schedule = schedule;
        Location = location;
        Status = EventStatus.Draft;
        CategoryId = categoryId;

        Capacity = capacity;

        Raise(new EventCreated(Id));
    }

    public void UpdateDetails(
        string title,
        string description,
        DateTimeRange schedule,
        Location location,
        int? capacity)
    {
        if (string.IsNullOrEmpty(title))
            throw new ValidationException("Title is required.");
        if (schedule == null)
            throw new ValidationException("Schedule is required.");
        if (location == null)
            throw new ValidationException("Location is required.");

        if (Status != EventStatus.Draft)
        {
            throw new DomainException("Only draft events can be updated.");
        }

        Title = title;
        Description = description;
        Schedule = schedule;
        Location = location;

        if (capacity != null)
        {
            Capacity = capacity;
        }

        //Raise(new EventDetailsUpdated(Id, Title, Description, Schedule, Venue));
    }

    public void Publish()
    {
        if (Status != EventStatus.Draft)
            throw new DomainException("Only draft events can be published.");

        if (!ticketTypes.Any())
            throw new DomainException("An event must have at least one ticket type to be published.");

        Status = EventStatus.Published;

        Raise(new EventPublished(Id));
    }

    public void Reschedule(DateTimeRange newSchedule)
    {

        if (newSchedule.Start < DateTime.Now)
            throw new DomainException("New schedule must be in the future.");

        Schedule = newSchedule;

        Raise(new EventRescheduled(Id, Schedule));
    }

    public void Cancel()
    {
        if (Status == EventStatus.Completed)
            throw new DomainException("Completed events cannot be canceled.");

        if (Schedule.Start <= DateTime.UtcNow)
            throw new DomainException("Events that have already started cannot be canceled.");

        Status = EventStatus.Canceled;

        Raise(new EventCanceled(Id));
    }

    public void AddTicketType(string ticketTypeName, Money price, int quantity)
    {
        if (ticketTypes.Any(t => t.Name == ticketTypeName))
            throw new DomainException("A ticket type with the same name already exists.");

        if (Capacity != null && quantity > Capacity)
            throw new DomainException("Ticket quantity cannot exceed event capacity.");

        var ticketType = new TicketType(ticketTypeName, price, quantity);

        ticketTypes.Add(ticketType);

        Raise(new TicketTypeAddedToEvent(Id, ticketType.Id));
    }
}
