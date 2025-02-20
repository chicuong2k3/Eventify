namespace Common.Domain;

public class PaginationResult<T> where T : class
{
    public PaginationResult(int pageNumber, int pageSize, int totalRecords, IReadOnlyCollection<T> data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        Data = data;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public int Pages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    public int TotalRecords { get; }
    public IReadOnlyCollection<T> Data { get; } = new List<T>();


}
