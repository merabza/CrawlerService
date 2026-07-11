using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateSchemeCommand(SchemeDto Scheme) : ICommand<bool>;