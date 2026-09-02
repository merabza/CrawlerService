using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.V1.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Serilog;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public static class HostEndpoints
{
    public static bool MapHostEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(MapHostEndpoints));

        RouteGroupBuilder group = endpoints
            .MapGroup(CrawlerServiceApiRoutes.ApiBase + CrawlerServiceApiRoutes.HostRoute.HostBase)
            .RequireAuthorization();

        group.MapGet(CrawlerServiceApiRoutes.HostRoute.List, GetHostsList);
        group.MapGet(CrawlerServiceApiRoutes.HostRoute.GetByName, GetHostByName);
        group.MapPost(CrawlerServiceApiRoutes.HostRoute.Create, CreateHost);
        group.MapPut(CrawlerServiceApiRoutes.HostRoute.Update, UpdateHost);
        group.MapDelete(CrawlerServiceApiRoutes.HostRoute.Delete, DeleteHost);

        debugLogger?.Information("{MethodName} Finished", nameof(MapHostEndpoints));

        return true;
    }

    private static async Task<IResult> GetHostsList(IQueryHandler<GetHostsListQuery, List<HostDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetHostsListQuery(), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> GetHostByName([FromQuery] string name,
        IQueryHandler<GetHostByNameQuery, ApiNullableResult<HostDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetHostByNameQuery(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> CreateHost([FromBody] HostDto host,
        ICommandHandler<CreateHostCommand, HostDto> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new CreateHostCommand(host), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> UpdateHost([FromBody] HostDto host,
        ICommandHandler<UpdateHostCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new UpdateHostCommand(host), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> DeleteHost([FromQuery] string name,
        ICommandHandler<DeleteHostCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new DeleteHostCommand(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }
}
