namespace EventCatalog.Domain.EventAggregate;

public record DateTimeRange
{
    public DateTime Start { get; private set; }
    public DateTime? End { get; private set; }

    public DateTimeRange(DateTime start, DateTime? end)
    {
        if (end != null && start >= end)
            throw new ValidationException("Start time must be before end time.");

        Start = start;
        End = end;
    }

    public bool Overlaps(DateTimeRange other)
    {
        return Start < other.End && End > other.Start;
    }
}
