using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateStartPointCommand(TaskStartPointDto StartPoint) : ICommand<bool>;
