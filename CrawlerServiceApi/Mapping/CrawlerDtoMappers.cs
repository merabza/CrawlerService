using System.Linq;
using CrawlerDbModels;
using CrawlerServiceShared.Contracts;

namespace CrawlerServiceApi.Mapping;

internal static class CrawlerDtoMappers
{
    #region Task

    public static TaskDto ToDto(this TaskModel task)
    {
        return new TaskDto
        {
            TaskId = task.TaskId,
            TaskName = task.TaskName,
            StartPoints = task.StartPoints.Select(sp => sp.ToDto()).ToList()
        };
    }

    public static TaskStartPointDto ToDto(this TaskStartPoint startPoint)
    {
        return new TaskStartPointDto
        {
            TspId = startPoint.TspId, TaskId = startPoint.TaskId, StartPoint = startPoint.StartPoint
        };
    }

    //ახალი ამოცანა იქმნება Start Point-ებითურთ (rename-ის დროს მათ შენარჩუნებას უზრუნველყოფს)
    public static TaskModel ToCreateEntity(this TaskDto dto)
    {
        return new TaskModel
        {
            TaskName = dto.TaskName,
            StartPoints = dto.StartPoints.Select(sp => new TaskStartPoint { StartPoint = sp.StartPoint }).ToList()
        };
    }

    //განახლებისას მხოლოდ სკალარული ველები ეხება, Start Point-ები ხელუხლებელი რჩება
    public static TaskModel ToUpdateEntity(this TaskDto dto)
    {
        return new TaskModel { TaskId = dto.TaskId, TaskName = dto.TaskName };
    }

    #endregion

    #region Batch

    public static BatchDto ToDto(this Batch batch)
    {
        return new BatchDto
        {
            BatchId = batch.BatchId,
            BatchName = batch.BatchName,
            IsOpen = batch.IsOpen,
            AutoCreateNextPart = batch.AutoCreateNextPart
        };
    }

    public static Batch ToEntity(this BatchDto dto)
    {
        return new Batch
        {
            BatchId = dto.BatchId,
            BatchName = dto.BatchName,
            IsOpen = dto.IsOpen,
            AutoCreateNextPart = dto.AutoCreateNextPart
        };
    }

    #endregion

    #region Host

    public static HostDto ToDto(this HostModel host)
    {
        return new HostDto { HostId = host.HostId, HostName = host.HostName, HostProhibited = host.HostProhibited };
    }

    public static HostModel ToEntity(this HostDto dto)
    {
        return new HostModel { HostId = dto.HostId, HostName = dto.HostName, HostProhibited = dto.HostProhibited };
    }

    #endregion

    #region Scheme

    public static SchemeDto ToDto(this SchemeModel scheme)
    {
        return new SchemeDto { SchId = scheme.SchId, SchName = scheme.SchName, SchProhibited = scheme.SchProhibited };
    }

    public static SchemeModel ToEntity(this SchemeDto dto)
    {
        return new SchemeModel { SchId = dto.SchId, SchName = dto.SchName, SchProhibited = dto.SchProhibited };
    }

    #endregion
}
