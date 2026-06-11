using System;
using System.Reflection;
using CrawlerService.DependencyInjection;
using CrawlerServiceApi;
using CrawlerServiceApi.DependencyInjection;
using CrawlerServiceReCounters.DependencyInjection;
using Figgle.Fonts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SystemTools.ReCounterAbstraction.DependencyInjection;
using WebSystemTools.ApiExceptionHandler.DependencyInjection;
using WebSystemTools.ApiKeyIdentity.DependencyInjection;
using WebSystemTools.MediatorTools.DependencyInjection;
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
        .AddReCounterAbstraction(debugLogger) //ReCounter
        //WebSystemTools
        .AddSwagger(debugLogger, true, versionCount, appName)
        .AddMediator(debugLogger,
            builder.Configuration,
            AssemblyReference.Assembly,
            WebSystemTools.SignalRRecounterMessages.AssemblyReference.Assembly)
        .AddSignalRRecounterMessages(debugLogger)
        .AddApiKeyIdentity(debugLogger)

        .AddCrawlerServiceReCounters(debugLogger)
        .AddCrawlerServiceDb(debugLogger, builder.Configuration)
        .AddHttpClient();
    // @formatter:on

    // ReSharper disable once using
    await using var app = builder.Build();
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
