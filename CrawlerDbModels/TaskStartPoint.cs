using System;

namespace CrawlerDbModels;

public sealed class TaskStartPoint
{
    private TaskModel? _taskNavigation;

    public int TspId { get; set; }
    public int TaskId { get; set; }
    public required string StartPoint { get; set; }

    public TaskModel TaskNavigation
    {
        get =>
            _taskNavigation ?? throw new InvalidOperationException("Uninitialized property: " + nameof(TaskNavigation));
        set => _taskNavigation = value;
    }
}
