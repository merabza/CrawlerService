using CrawlerDbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrawlerDbPersistence.Configurations;

public sealed class TaskModelConfiguration : IEntityTypeConfiguration<TaskModel>
{
    private const int TaskNameLength = 50;

    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        const string tableName = "Tasks";
        builder.ToTable(tableName);

        builder.HasKey(e => e.TaskId);
        builder.HasIndex(e => e.TaskName).IsUnique();

        builder.Property(e => e.TaskName).HasMaxLength(TaskNameLength);
    }
}
