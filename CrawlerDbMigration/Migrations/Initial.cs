#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrawlerDbMigration.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Batches",
                table => new
                {
                    BatchId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    IsOpen = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    AutoCreateNextPart = table.Column<bool>("bit", nullable: false, defaultValue: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.BatchId);
                });

            migrationBuilder.CreateTable("Extensions",
                table => new
                {
                    ExtId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    ExtName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    ExtProhibited = table.Column<bool>("bit", nullable: false, defaultValue: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Extensions", x => x.ExtId);
                });

            migrationBuilder.CreateTable("Hosts",
                table => new
                {
                    HostId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    HostName = table.Column<string>("nvarchar(253)", maxLength: 253, nullable: false),
                    HostProhibited = table.Column<bool>("bit", nullable: false, defaultValue: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.HostId);
                });

            migrationBuilder.CreateTable("Schemes",
                table => new
                {
                    SchId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    SchName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    SchProhibited = table.Column<bool>("bit", nullable: false, defaultValue: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Schemes", x => x.SchId);
                });

            migrationBuilder.CreateTable("Tasks",
                table => new
                {
                    TaskId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                });

            migrationBuilder.CreateTable("TermTypes",
                table => new
                {
                    TtId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    TtKey = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    TtName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_TermTypes", x => x.TtId);
                });

            migrationBuilder.CreateTable("BatchParts",
                table => new
                {
                    BpId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>("int", nullable: false),
                    Created = table.Column<DateTime>("datetime", nullable: false),
                    Finished = table.Column<DateTime>("datetime", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_BatchParts", x => x.BpId);
                    table.ForeignKey("FK_BatchParts_Batches_BatchId", x => x.BatchId, "Batches", "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("HostsByBatches",
                table => new
                {
                    HbbId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>("int", nullable: false),
                    SchemeId = table.Column<int>("int", nullable: false),
                    HostId = table.Column<int>("int", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_HostsByBatches", x => x.HbbId);
                    table.ForeignKey("FK_HostsByBatches_Batches_BatchId", x => x.BatchId, "Batches", "BatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_HostsByBatches_Hosts_HostId", x => x.HostId, "Hosts", "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_HostsByBatches_Schemes_SchemeId", x => x.SchemeId, "Schemes", "SchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("Urls",
                table => new
                {
                    UrlId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UrlName = table.Column<string>("nvarchar(2048)", maxLength: 2048, nullable: false),
                    HostId = table.Column<int>("int", nullable: false),
                    ExtensionId = table.Column<int>("int", nullable: false),
                    SchemeId = table.Column<int>("int", nullable: false),
                    UrlHashCode = table.Column<int>("int", nullable: false),
                    IsSiteMap = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    IsAllowed = table.Column<bool>("bit", nullable: false, defaultValue: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.UrlId);
                    table.ForeignKey("FK_Urls_Extensions_ExtensionId", x => x.ExtensionId, "Extensions", "ExtId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Urls_Hosts_HostId", x => x.HostId, "Hosts", "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Urls_Schemes_SchemeId", x => x.SchemeId, "Schemes", "SchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("TaskStartPoints",
                table => new
                {
                    TspId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>("int", nullable: false),
                    StartPoint = table.Column<string>("nvarchar(2048)", maxLength: 2048, nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStartPoints", x => x.TspId);
                    table.ForeignKey("FK_TaskStartPoints_Tasks_TaskId", x => x.TaskId, "Tasks", "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("Terms",
                table => new
                {
                    TrmId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    TermText = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    termTypeId = table.Column<int>("int", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TrmId);
                    table.ForeignKey("FK_Terms_TermTypes_termTypeId", x => x.termTypeId, "TermTypes", "TtId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("Robots",
                table => new
                {
                    RbtId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>("int", nullable: false),
                    SchemeId = table.Column<int>("int", nullable: false),
                    HostId = table.Column<int>("int", nullable: false),
                    RobotsTxt = table.Column<string>("ntext", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Robots", x => x.RbtId);
                    table.ForeignKey("FK_Robots_BatchParts_BatchPartId", x => x.BatchPartId, "BatchParts", "BpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Robots_Hosts_HostId", x => x.HostId, "Hosts", "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Robots_Schemes_SchemeId", x => x.SchemeId, "Schemes", "SchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("ContentsAnalysis",
                table => new
                {
                    CaId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>("int", nullable: false),
                    UrlId = table.Column<int>("int", nullable: false),
                    ResponseStatusCode = table.Column<int>("int", nullable: false),
                    Finish = table.Column<DateTime>("datetime", nullable: false),
                    LastModifiedDateOnServer = table.Column<DateTime>("datetime", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_ContentsAnalysis", x => x.CaId);
                    table.ForeignKey("FK_ContentsAnalysis_BatchParts_BatchPartId", x => x.BatchPartId, "BatchParts",
                        "BpId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_ContentsAnalysis_Urls_UrlId", x => x.UrlId, "Urls", "UrlId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("UrlGraphNodes",
                table => new
                {
                    UgnId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>("int", nullable: false),
                    FromUrlId = table.Column<int>("int", nullable: false),
                    GotUrlId = table.Column<int>("int", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_UrlGraphNodes", x => x.UgnId);
                    table.ForeignKey("FK_UrlGraphNodes_BatchParts_BatchPartId", x => x.BatchPartId, "BatchParts",
                        "BpId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_UrlGraphNodes_Urls_FromUrlId", x => x.FromUrlId, "Urls", "UrlId");
                    table.ForeignKey("FK_UrlGraphNodes_Urls_GotUrlId", x => x.GotUrlId, "Urls", "UrlId");
                });

            migrationBuilder.CreateTable("TermsByUrls",
                table => new
                {
                    TbuId = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>("int", nullable: false),
                    UrlId = table.Column<int>("int", nullable: false),
                    TermId = table.Column<int>("int", nullable: false),
                    Position = table.Column<int>("int", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_TermsByUrls", x => x.TbuId);
                    table.ForeignKey("FK_TermsByUrls_BatchParts_BatchPartId", x => x.BatchPartId, "BatchParts", "BpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_TermsByUrls_Terms_TermId", x => x.TermId, "Terms", "TrmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_TermsByUrls_Urls_UrlId", x => x.UrlId, "Urls", "UrlId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_Batches_BatchName", "Batches", "BatchName", unique: true);

            migrationBuilder.CreateIndex("IX_BatchParts_BatchId_Created", "BatchParts", new[] { "BatchId", "Created" },
                unique: true);

            migrationBuilder.CreateIndex("IX_ContentsAnalysis_BatchPartId_UrlId", "ContentsAnalysis",
                new[] { "BatchPartId", "UrlId" }, unique: true);

            migrationBuilder.CreateIndex("IX_ContentsAnalysis_UrlId", "ContentsAnalysis", "UrlId");

            migrationBuilder.CreateIndex("IX_Extensions_ExtName", "Extensions", "ExtName", unique: true);

            migrationBuilder.CreateIndex("IX_Hosts_HostName", "Hosts", "HostName", unique: true);

            migrationBuilder.CreateIndex("IX_HostsByBatches_BatchId_SchemeId_HostId", "HostsByBatches",
                new[] { "BatchId", "SchemeId", "HostId" }, unique: true);

            migrationBuilder.CreateIndex("IX_HostsByBatches_HostId", "HostsByBatches", "HostId");

            migrationBuilder.CreateIndex("IX_HostsByBatches_SchemeId", "HostsByBatches", "SchemeId");

            migrationBuilder.CreateIndex("IX_Robots_BatchPartId_SchemeId_HostId", "Robots",
                new[] { "BatchPartId", "SchemeId", "HostId" }, unique: true);

            migrationBuilder.CreateIndex("IX_Robots_HostId", "Robots", "HostId");

            migrationBuilder.CreateIndex("IX_Robots_SchemeId", "Robots", "SchemeId");

            migrationBuilder.CreateIndex("IX_Schemes_SchName", "Schemes", "SchName", unique: true);

            migrationBuilder.CreateIndex("IX_Tasks_TaskName", "Tasks", "TaskName", unique: true);

            migrationBuilder.CreateIndex("IX_TaskStartPoints_TaskId", "TaskStartPoints", "TaskId");

            migrationBuilder.CreateIndex("IX_Terms_TermText", "Terms", "TermText");

            migrationBuilder.CreateIndex("IX_Terms_termTypeId", "Terms", "termTypeId");

            migrationBuilder.CreateIndex("IX_TermsByUrls_BatchPartId_UrlId_Position", "TermsByUrls",
                new[] { "BatchPartId", "UrlId", "Position" }, unique: true);

            migrationBuilder.CreateIndex("IX_TermsByUrls_TermId", "TermsByUrls", "TermId");

            migrationBuilder.CreateIndex("IX_TermsByUrls_UrlId", "TermsByUrls", "UrlId");

            migrationBuilder.CreateIndex("IX_TermTypes_TtKey", "TermTypes", "TtKey", unique: true);

            migrationBuilder.CreateIndex("IX_UrlGraphNodes_BatchPartId_FromUrlId_GotUrlId", "UrlGraphNodes",
                new[] { "BatchPartId", "FromUrlId", "GotUrlId" }, unique: true);

            migrationBuilder.CreateIndex("IX_UrlGraphNodes_FromUrlId", "UrlGraphNodes", "FromUrlId");

            migrationBuilder.CreateIndex("IX_UrlGraphNodes_GotUrlId", "UrlGraphNodes", "GotUrlId");

            migrationBuilder.CreateIndex("IX_Urls_ExtensionId", "Urls", "ExtensionId");

            migrationBuilder.CreateIndex("IX_Urls_HostId", "Urls", "HostId");

            migrationBuilder.CreateIndex("IX_Urls_SchemeId", "Urls", "SchemeId");

            migrationBuilder.CreateIndex("IX_Urls_UrlHashCode_HostId_ExtensionId_SchemeId", "Urls",
                new[] { "UrlHashCode", "HostId", "ExtensionId", "SchemeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ContentsAnalysis");

            migrationBuilder.DropTable("HostsByBatches");

            migrationBuilder.DropTable("Robots");

            migrationBuilder.DropTable("TaskStartPoints");

            migrationBuilder.DropTable("TermsByUrls");

            migrationBuilder.DropTable("UrlGraphNodes");

            migrationBuilder.DropTable("Tasks");

            migrationBuilder.DropTable("Terms");

            migrationBuilder.DropTable("BatchParts");

            migrationBuilder.DropTable("Urls");

            migrationBuilder.DropTable("TermTypes");

            migrationBuilder.DropTable("Batches");

            migrationBuilder.DropTable("Extensions");

            migrationBuilder.DropTable("Hosts");

            migrationBuilder.DropTable("Schemes");
        }
    }
}
