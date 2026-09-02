using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateSchemeCommand(SchemeDto Scheme) : ICommand<bool>;
