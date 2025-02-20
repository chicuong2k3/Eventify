namespace EventCatalog.Domain.EventAggregate;

public class EventSpecification : SpecificationBase
{
    public Guid? CategoryId { get; }
    public string? SearchText { get; }
    public int? MaxCapacity { get; }
    public DateTimeRange? Schedule { get; }
    public string? SortBy { get; }

    public EventSpecification(
        int pageSize,
        int pageNumber,
        Guid? categoryId,
        string? searchText,
        int? maxCapacity,
        DateTimeRange? schedule,
        string? sortBy)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        CategoryId = categoryId;
        SearchText = searchText;
        MaxCapacity = maxCapacity;
        Schedule = schedule;
        SortBy = sortBy;
    }
}
