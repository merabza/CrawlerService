using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CrawlerServiceReCounters.DependencyInjection;

// ReSharper disable once UnusedType.Global
public static class CrawlerServiceReCountersDependencyInjection
{
    public static IServiceCollection AddCrawlerServiceReCounters(this IServiceCollection services, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(AddCrawlerServiceReCounters));

        // CrawlerReCounter is created explicitly by the command handlers; no extra registrations are required here.

        debugLogger?.Information("{MethodName} Finished", nameof(AddCrawlerServiceReCounters));

        return services;
    }
}
