using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record CreateSchemeCommand(SchemeDto Scheme) : ICommand<SchemeDto>;