using CrawlerServiceApi.Endpoints.V1;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace CrawlerServiceApi.DependencyInjection;

public static class CrawlerServiceApiDependencyInjection
{
    public static bool UseCrawlerApiEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(UseCrawlerApiEndpoints));

        endpoints.MapCrawlerEndpoints(debugLogger);
        endpoints.MapTaskEndpoints(debugLogger);
        endpoints.MapBatchEndpoints(debugLogger);
        endpoints.MapHostEndpoints(debugLogger);
        endpoints.MapSchemeEndpoints(debugLogger);

        debugLogger?.Information("{MethodName} Finished", nameof(UseCrawlerApiEndpoints));

        return true;
    }
}
