namespace EventCatalog.Domain.CategoryAggregate;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Category category);
    void Remove(Category category);

    Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);
}
