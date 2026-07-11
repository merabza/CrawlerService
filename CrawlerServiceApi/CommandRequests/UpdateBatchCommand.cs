using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateBatchCommand(BatchDto Batch) : ICommand<bool>;