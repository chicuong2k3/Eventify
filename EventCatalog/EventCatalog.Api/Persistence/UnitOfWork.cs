namespace EventCatalog.Api.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EventCatalogDbContext dbContext;

    public UnitOfWork(EventCatalogDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
