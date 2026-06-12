using System;
using System.Net.Http;
using CrawlerDb;
using DoCrawler;
using DoCrawler.Models;
using LibCrawlerRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SystemTools.DependencyInjection;
using SystemTools.SystemToolsShared;

namespace CrawlerService.DependencyInjection;

public static class CrawlerServiceDbDependencyInjection
{
    public static IServiceCollection AddCrawlerServiceDb(this IServiceCollection services, ILogger? debugLogger,
        IConfiguration configuration)
    {
        debugLogger?.Information("{MethodName} Started", nameof(AddCrawlerServiceDb));

        // 1. Crawl parsing parameters (alphabet, punctuations, ...) are loaded from the crawler parameters file.
        var parametersLoader = CrawlerParameters.Create(configuration);
        if (parametersLoader != null)
        {
            services.AddSingleton(parametersLoader);
        }
        else
        {
            Console.WriteLine("Cannot load CrawlerParameters");
        }

        // 2. Database connection comes from configuration (Data:CrawlerServiceDatabase).
        string? connectionString = configuration["Data:CrawlerServiceDatabase:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Console.WriteLine("Crawler database connection string is empty");
        }
        else
        {
            int commandTimeout = int.TryParse(configuration["Data:CrawlerServiceDatabase:CommandTimeOut"],
                out int parsedTimeout)
                ? parsedTimeout
                : -1;
            services.AddContextByProvider<CrawlerDbContext>(EDatabaseProvider.SqlServer, connectionString,
                commandTimeout);
        }

        // 3. Repositories and the named crawler HttpClient (redirects disabled, as the crawler tracks them itself).
        services.AddSingleton<ICrawlerRepositoryCreatorFactory, CrawlerRepositoryCreatorFactory>()
            .AddScoped<ICrawlerRepository, CrawlerRepository>().AddHttpClient(BatchPartRunner.CrawlerClient)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

        debugLogger?.Information("{MethodName} Finished", nameof(AddCrawlerServiceDb));

        return services;
    }
}
