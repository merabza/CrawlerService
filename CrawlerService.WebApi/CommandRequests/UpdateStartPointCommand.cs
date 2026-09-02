using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record UpdateStartPointCommand(TaskStartPointDto StartPoint) : ICommand<bool>;
