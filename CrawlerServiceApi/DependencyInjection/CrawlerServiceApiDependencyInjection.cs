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

        debugLogger?.Information("{MethodName} Finished", nameof(UseCrawlerApiEndpoints));

        return true;
    }
}
