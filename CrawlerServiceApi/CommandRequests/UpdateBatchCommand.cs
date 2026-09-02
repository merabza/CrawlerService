using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateBatchCommand(BatchDto Batch) : ICommand<bool>;
