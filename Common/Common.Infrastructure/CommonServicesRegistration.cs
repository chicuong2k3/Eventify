using Common.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure;

public static class CommonServicesRegistration
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyLookup, CurrencyLookup>();
        return services;
    }
}
