// ReSharper disable CollectionNeverUpdated.Global

using System.Collections.Generic;
using SystemTools.SystemToolsShared;

namespace CrawlerDbModels;

public sealed class TaskModel : ItemData
{
    public int TaskId { get; init; }
    public required string TaskName { get; init; }
    public ICollection<TaskStartPoint> StartPoints { get; init; } = new HashSet<TaskStartPoint>();
}
