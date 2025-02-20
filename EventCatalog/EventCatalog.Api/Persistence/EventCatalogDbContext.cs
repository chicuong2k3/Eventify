namespace EventCatalog.Api.Persistence;

public sealed class EventCatalogDbContext : DbContext, IUnitOfWork
{
    private readonly ILogger<EventCatalogDbContext> logger;

    public EventCatalogDbContext(
        DbContextOptions<EventCatalogDbContext> options,
        ILogger<EventCatalogDbContext> logger)
        : base(options)
    {
        this.logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventCatalogDbContext).Assembly);
    }

    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Event> Events { get; set; } = default!;


}
