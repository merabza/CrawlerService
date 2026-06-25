using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetBatchesListQuery : IQuery<List<BatchDto>>;

public sealed record GetBatchByNameQuery(string Name) : IQuery<ApiNullableResult<BatchDto>>;

public sealed record CreateBatchCommand(BatchDto Batch) : ICommand<BatchDto>;

public sealed record UpdateBatchCommand(BatchDto Batch) : ICommand<bool>;

public sealed record DeleteBatchCommand(string Name) : ICommand<bool>;

public sealed record GetHostStartUrlNamesByBatchQuery(string BatchName) : IQuery<List<string>>;

public sealed record AddHostByBatchCommand(string BatchName, string SchemeName, string HostName) : ICommand<bool>;

public sealed record RemoveHostByBatchCommand(string BatchName, string SchemeName, string HostName) : ICommand<bool>;
