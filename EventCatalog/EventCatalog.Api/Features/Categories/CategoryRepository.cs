using EventCatalog.Api.Persistence;

namespace EventCatalog.Api.Features.Categories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly EventCatalogDbContext dbContext;

    public CategoryRepository(EventCatalogDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Category category)
    {
        dbContext.Categories.Add(category);
    }

    public async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories.ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories.FindAsync(id, cancellationToken);
    }

    public void Remove(Category category)
    {
        dbContext.Categories.Remove(category);
    }
}
