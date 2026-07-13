using CrawlerDbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SystemTools.SystemToolsShared;

namespace CrawlerDbPersistence.Configurations;

public sealed class TermConfiguration : IEntityTypeConfiguration<Term>
{
    public const int TermTextLength = 50;

    public void Configure(EntityTypeBuilder<Term> builder)
    {
        //string tableName = nameof(Term).Pluralize();
        //builder.ToTable(tableName);

        builder.HasKey(e => e.TrmId);
        builder.HasIndex(e => e.TermText);
        builder.Property(e => e.TermText).HasMaxLength(TermTextLength); //.UseCollation("SQL_Latin1_General_CP1_CS_AS");
        builder.Property(e => e.TermTypeId).HasColumnName(nameof(Term.TermTypeId).UnCapitalize());

        builder.HasOne(d => d.TermTypeNavigation).WithMany(p => p.Terms).HasForeignKey(d => d.TermTypeId);
    }
}
