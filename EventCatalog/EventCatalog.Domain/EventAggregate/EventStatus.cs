namespace EventCatalog.Domain.EventAggregate
{

    public enum EventStatus
    {
        Draft = 0,
        Published = 1,
        Canceled = 2,
        Completed = 3,
    }
}
