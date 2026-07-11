using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateStartPointCommand(TaskStartPointDto StartPoint) : ICommand<bool>;