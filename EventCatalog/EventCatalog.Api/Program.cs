using Serilog;
using EventCatalog.Api;
using EventCatalog.Api.Features.Categories;
using System.Reflection;
using Asp.Versioning;
using EventCatalog.Api.Persistence;
using EventCatalog.Api.Features.Events;
using Common.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCommonServices();

builder.Services.AddControllers();

// Add Logging
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "EventCatalog.Api",
        Version = "v1"
    });
});

// Add Api Versioning
builder.Services.AddApiVersioning(options =>
 {
     options.ReportApiVersions = true;
     options.AssumeDefaultVersionWhenUnspecified = true;
     options.DefaultApiVersion = new ApiVersion(1, 0);
     //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
 });

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Persistence
var dbConnectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("'Database' connection string cannot be null or empty.");
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache");

builder.Services.AddDbContext<EventCatalogDbContext>(options =>
{
    options.UseNpgsql(dbConnectionString)
        .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Add Health Checks
//builder.Services.AddHealthChecks()
//    .AddNpgSql(dbConnectionString)
//    .AddRedis(cacheConnectionString)
//    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!),
//    HttpMethod.Get,
//    "key-cloak");



builder.Services.AddMediatR(configure =>
{
    configure.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});



var app = builder.Build();

app.Services.MigrateDatabase();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

//app.MapHealthChecks("health", new HealthCheckOptions()
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();



//public static class ServiceCollectionExtensions
//{
//    public static void AddDomainEventHandlers(this IServiceCollection services)
//    {
//        var domainEventHandlers = Assembly.GetExecutingAssembly().GetTypes()
//            .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
//            .ToArray();

//        foreach (var domainEventHandler in domainEventHandlers)
//        {
//            services.TryAddScoped(domainEventHandler);

//            var domainEvent = domainEventHandler
//                .GetInterfaces()
//                .Single(x => x.IsGenericType)
//                .GetGenericArguments()
//                .Single();

//            //var closedIdempotentDomainEventHandler = typeof(IdempotentDomainEventHandler<>)
//            //    .MakeGenericType(domainEvent);

//            //services.Decorate(domainEventHandler, closedIdempotentDomainEventHandler);
//        }
//    }
//}