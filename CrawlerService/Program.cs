using System;
using System.Reflection;
using CrawlerService.Application.DependencyInjection;
using CrawlerService.DependencyInjection;
using CrawlerService.WebApi;
using CrawlerService.WebApi.DependencyInjection;
using Figgle.Fonts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SystemTools.Application.Abstractions;
using SystemTools.ReCounterAbstraction.DependencyInjection;
using WebSystemTools.ApiExceptionHandler.DependencyInjection;
using WebSystemTools.ApiKeyIdentity.DependencyInjection;
using WebSystemTools.SerilogLogger;
using WebSystemTools.SignalRRecounterMessages.DependencyInjection;
using WebSystemTools.SwaggerTools.DependencyInjection;
using WebSystemTools.TestToolsApi.DependencyInjection;
using WebSystemTools.WindowsServiceTools;

try
{
    Console.WriteLine("Loading...");

    const string appName = "Crawler Service";
    const int versionCount = 1;

    string header = $"{appName} {Assembly.GetEntryAssembly()?.GetName().Version}";
    Console.WriteLine(FiggleFonts.Standard.Render(header));

    WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        ContentRootPath = AppContext.BaseDirectory, Args = args
    });

    bool debugMode = builder.Environment.IsDevelopment();

    ILogger logger = builder.Host.UseSerilogLogger(debugMode, builder.Configuration);
    ILogger? debugLogger = debugMode ? logger : null;
    builder.Host.UseWindowsServiceOnWindows(debugLogger, args);

    // @formatter:off
    builder.Services
        //SystemTools
        .AddApplication(debugLogger,
            typeof(AssemblyReference),
            typeof(WebSystemTools.SignalRRecounterMessages.AssemblyReference))
        .AddReCounterAbstraction(debugLogger) //ReCounter
        //WebSystemTools
        .AddSwagger(debugLogger, true, versionCount, appName)
        .AddSignalRRecounterMessages(debugLogger)
        .AddApiKeyIdentity(debugLogger)

        .AddCrawlerServiceApplication(debugLogger, builder.Configuration)
        .AddCrawlerServiceDb(debugLogger, builder.Configuration)
        .AddHttpClient();
    // @formatter:on

    // ReSharper disable once using
    await using WebApplication app = builder.Build();
    app.UseSwaggerServices(debugLogger);
    app.UseApiKeysAuthorization(debugLogger);
    app.UseTestToolsApiEndpoints(debugLogger);
    app.UseSignalRRecounterMessages(debugLogger);
    app.UseCrawlerApiEndpoints(debugLogger);
    app.UseApiExceptionHandler(debugLogger);

    await app.RunAsync();
    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
