using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record CreateSchemeCommand(SchemeDto Scheme) : ICommand<SchemeDto>;
