using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrawlerDbMigration.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    BatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AutoCreateNextPart = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "Extensions",
                columns: table => new
                {
                    ExtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExtProhibited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extensions", x => x.ExtId);
                });

            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    HostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostName = table.Column<string>(type: "nvarchar(253)", maxLength: 253, nullable: false),
                    HostProhibited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.HostId);
                });

            migrationBuilder.CreateTable(
                name: "Schemes",
                columns: table => new
                {
                    SchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchProhibited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schemes", x => x.SchId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApiName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "TermTypes",
                columns: table => new
                {
                    TtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TtKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TtName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermTypes", x => x.TtId);
                });

            migrationBuilder.CreateTable(
                name: "BatchParts",
                columns: table => new
                {
                    BpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Finished = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchParts", x => x.BpId);
                    table.ForeignKey(
                        name: "FK_BatchParts_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HostsByBatches",
                columns: table => new
                {
                    HbbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    SchemeId = table.Column<int>(type: "int", nullable: false),
                    HostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostsByBatches", x => x.HbbId);
                    table.ForeignKey(
                        name: "FK_HostsByBatches_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HostsByBatches_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HostsByBatches_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalTable: "Schemes",
                        principalColumn: "SchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    UrlId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlName = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    ExtensionId = table.Column<int>(type: "int", nullable: false),
                    SchemeId = table.Column<int>(type: "int", nullable: false),
                    UrlHashCode = table.Column<int>(type: "int", nullable: false),
                    IsSiteMap = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.UrlId);
                    table.ForeignKey(
                        name: "FK_Urls_Extensions_ExtensionId",
                        column: x => x.ExtensionId,
                        principalTable: "Extensions",
                        principalColumn: "ExtId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Urls_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Urls_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalTable: "Schemes",
                        principalColumn: "SchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskStartPoints",
                columns: table => new
                {
                    TspId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    StartPoint = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStartPoints", x => x.TspId);
                    table.ForeignKey(
                        name: "FK_TaskStartPoints_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    TrmId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    termTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TrmId);
                    table.ForeignKey(
                        name: "FK_Terms_TermTypes_termTypeId",
                        column: x => x.termTypeId,
                        principalTable: "TermTypes",
                        principalColumn: "TtId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Robots",
                columns: table => new
                {
                    RbtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>(type: "int", nullable: false),
                    SchemeId = table.Column<int>(type: "int", nullable: false),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    RobotsTxt = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Robots", x => x.RbtId);
                    table.ForeignKey(
                        name: "FK_Robots_BatchParts_BatchPartId",
                        column: x => x.BatchPartId,
                        principalTable: "BatchParts",
                        principalColumn: "BpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Robots_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Robots_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalTable: "Schemes",
                        principalColumn: "SchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentsAnalysis",
                columns: table => new
                {
                    CaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>(type: "int", nullable: false),
                    UrlId = table.Column<int>(type: "int", nullable: false),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: false),
                    Finish = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModifiedDateOnServer = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentsAnalysis", x => x.CaId);
                    table.ForeignKey(
                        name: "FK_ContentsAnalysis_BatchParts_BatchPartId",
                        column: x => x.BatchPartId,
                        principalTable: "BatchParts",
                        principalColumn: "BpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentsAnalysis_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "UrlId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrlGraphNodes",
                columns: table => new
                {
                    UgnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>(type: "int", nullable: false),
                    FromUrlId = table.Column<int>(type: "int", nullable: false),
                    GotUrlId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlGraphNodes", x => x.UgnId);
                    table.ForeignKey(
                        name: "FK_UrlGraphNodes_BatchParts_BatchPartId",
                        column: x => x.BatchPartId,
                        principalTable: "BatchParts",
                        principalColumn: "BpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrlGraphNodes_Urls_FromUrlId",
                        column: x => x.FromUrlId,
                        principalTable: "Urls",
                        principalColumn: "UrlId");
                    table.ForeignKey(
                        name: "FK_UrlGraphNodes_Urls_GotUrlId",
                        column: x => x.GotUrlId,
                        principalTable: "Urls",
                        principalColumn: "UrlId");
                });

            migrationBuilder.CreateTable(
                name: "TermsByUrls",
                columns: table => new
                {
                    TbuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchPartId = table.Column<int>(type: "int", nullable: false),
                    UrlId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsByUrls", x => x.TbuId);
                    table.ForeignKey(
                        name: "FK_TermsByUrls_BatchParts_BatchPartId",
                        column: x => x.BatchPartId,
                        principalTable: "BatchParts",
                        principalColumn: "BpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TermsByUrls_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "TrmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TermsByUrls_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "UrlId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_BatchName",
                table: "Batches",
                column: "BatchName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchParts_BatchId_Created",
                table: "BatchParts",
                columns: new[] { "BatchId", "Created" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentsAnalysis_BatchPartId_UrlId",
                table: "ContentsAnalysis",
                columns: new[] { "BatchPartId", "UrlId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentsAnalysis_UrlId",
                table: "ContentsAnalysis",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Extensions_ExtName",
                table: "Extensions",
                column: "ExtName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_HostName",
                table: "Hosts",
                column: "HostName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HostsByBatches_BatchId_SchemeId_HostId",
                table: "HostsByBatches",
                columns: new[] { "BatchId", "SchemeId", "HostId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HostsByBatches_HostId",
                table: "HostsByBatches",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_HostsByBatches_SchemeId",
                table: "HostsByBatches",
                column: "SchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Robots_BatchPartId_SchemeId_HostId",
                table: "Robots",
                columns: new[] { "BatchPartId", "SchemeId", "HostId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Robots_HostId",
                table: "Robots",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Robots_SchemeId",
                table: "Robots",
                column: "SchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Schemes_SchName",
                table: "Schemes",
                column: "SchName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskName",
                table: "Tasks",
                column: "TaskName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskStartPoints_TaskId",
                table: "TaskStartPoints",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Terms_TermText",
                table: "Terms",
                column: "TermText");

            migrationBuilder.CreateIndex(
                name: "IX_Terms_termTypeId",
                table: "Terms",
                column: "termTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TermsByUrls_BatchPartId_UrlId_Position",
                table: "TermsByUrls",
                columns: new[] { "BatchPartId", "UrlId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TermsByUrls_TermId",
                table: "TermsByUrls",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TermsByUrls_UrlId",
                table: "TermsByUrls",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_TermTypes_TtKey",
                table: "TermTypes",
                column: "TtKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlGraphNodes_BatchPartId_FromUrlId_GotUrlId",
                table: "UrlGraphNodes",
                columns: new[] { "BatchPartId", "FromUrlId", "GotUrlId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlGraphNodes_FromUrlId",
                table: "UrlGraphNodes",
                column: "FromUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_UrlGraphNodes_GotUrlId",
                table: "UrlGraphNodes",
                column: "GotUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ExtensionId",
                table: "Urls",
                column: "ExtensionId");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_HostId",
                table: "Urls",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_SchemeId",
                table: "Urls",
                column: "SchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UrlHashCode_HostId_ExtensionId_SchemeId",
                table: "Urls",
                columns: new[] { "UrlHashCode", "HostId", "ExtensionId", "SchemeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentsAnalysis");

            migrationBuilder.DropTable(
                name: "HostsByBatches");

            migrationBuilder.DropTable(
                name: "Robots");

            migrationBuilder.DropTable(
                name: "TaskStartPoints");

            migrationBuilder.DropTable(
                name: "TermsByUrls");

            migrationBuilder.DropTable(
                name: "UrlGraphNodes");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "BatchParts");

            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "TermTypes");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "Extensions");

            migrationBuilder.DropTable(
                name: "Hosts");

            migrationBuilder.DropTable(
                name: "Schemes");
        }
    }
}
