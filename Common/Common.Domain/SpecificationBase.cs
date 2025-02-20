namespace Common.Domain;

public abstract class SpecificationBase
{
    private const int maxPageSize = 50;
    private int pageSize;
    public int PageSize
    {
        get => pageSize;
        protected set => pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    public int PageNumber { get; protected set; } = 1;
}
