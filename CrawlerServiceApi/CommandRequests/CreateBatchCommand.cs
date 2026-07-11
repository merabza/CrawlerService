using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record CreateBatchCommand(BatchDto Batch) : ICommand<BatchDto>;