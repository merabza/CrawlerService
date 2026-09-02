using System;
using CrawlerServiceDbPart.Db;
using CrawlerServiceRoot.Application.Abstractions;
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

        // Database connection comes from configuration (Data:CrawlerServiceDatabase).
        string? databaseProvider = configuration["Data:CrawlerServiceDatabase:DatabaseProvider"];

        if (!Enum.TryParse(databaseProvider ?? string.Empty, true, out EDatabaseProvider result))
        {
            throw new InvalidOperationException($"Invalid database provider '{databaseProvider}'");
        }

        string? connectionString = configuration["Data:CrawlerServiceDatabase:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Crawler database connection string is empty");
        }

        string? commandTimeoutValue = configuration["Data:CrawlerServiceDatabase:CommandTimeOut"];
        int commandTimeout = -1;
        if (!string.IsNullOrWhiteSpace(commandTimeoutValue))
        {
            if (int.TryParse(commandTimeoutValue, out int parsedTimeout))
            {
                commandTimeout = parsedTimeout;
            }
            else
            {
                Log.Warning("Invalid CommandTimeOut value '{CommandTimeOutValue}', command timeout will not be set",
                    commandTimeoutValue);
            }
        }

        services.AddContextByProvider<CrawlerDbContext>(result, connectionString, commandTimeout);
        services.AddScoped<ICrawlerServiceApplicationDbContext>(sp => sp.GetRequiredService<CrawlerDbContext>());

        debugLogger?.Information("{MethodName} Finished", nameof(AddCrawlerServiceDb));

        return services;
    }
}
