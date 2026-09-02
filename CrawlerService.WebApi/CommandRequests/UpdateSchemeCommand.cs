using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record UpdateSchemeCommand(SchemeDto Scheme) : ICommand<bool>;
