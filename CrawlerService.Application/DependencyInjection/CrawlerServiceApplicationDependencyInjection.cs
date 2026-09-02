using System;
using System.Net.Http;
using CrawlerService.Application.Crawling;
using CrawlerService.Application.Crawling.Models;
using CrawlerService.Application.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CrawlerService.Application.DependencyInjection;

// ReSharper disable once UnusedType.Global
public static class CrawlerServiceApplicationDependencyInjection
{
    public static IServiceCollection AddCrawlerServiceApplication(this IServiceCollection services,
        ILogger? debugLogger, IConfiguration configuration)
    {
        debugLogger?.Information("{MethodName} Started", nameof(AddCrawlerServiceApplication));

        // 1. Crawl parsing parameters (alphabet, punctuations, ...) are loaded from the crawler parameters file.
        CrawlerParameters crawlerParameters = CrawlerParameters.Create(configuration) ??
                                              throw new InvalidOperationException(
                                                  "Cannot load CrawlerParameters section from configuration");

        services.AddSingleton(crawlerParameters);

        // 2. Repositories and the named crawler HttpClient (redirects disabled, as the crawler tracks them itself).
        services.AddSingleton<ICrawlerRepositoryCreatorFactory, CrawlerRepositoryCreatorFactory>()
            .AddScoped<ICrawlerRepository, CrawlerRepository>().AddHttpClient(BatchPartRunner.CrawlerClient)
            .ConfigureHttpClient(client =>
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; CrawlerService/1.0)"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

        // 3. CrawlerReCounter is created explicitly by the command handlers; no extra registrations are required here.

        debugLogger?.Information("{MethodName} Finished", nameof(AddCrawlerServiceApplication));

        return services;
    }
}
