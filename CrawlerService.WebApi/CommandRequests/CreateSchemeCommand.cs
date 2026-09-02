using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record CreateSchemeCommand(SchemeDto Scheme) : ICommand<SchemeDto>;
