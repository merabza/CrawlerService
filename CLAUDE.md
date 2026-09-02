# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout — sibling repos are required

This repo is one of six sibling clones that must sit next to each other, because the solution and the csproj files reference them by relative path (`..\..\SystemTools\...`):

```
D:\1WorkDotnet\CrawlerService\        <- solution tree root
├── CrawlerService\                   <- this repo (CrawlerService.slnx lives here)
├── CrawlerServiceRoot\               <- domain entities + ICrawlerServiceApplicationDbContext (merabza/CrawlerServiceRoot)
├── CrawlerServiceDbPart\             <- CrawlerDbContext + entity configurations (merabza/CrawlerServiceDbPart)
├── CrawlerServiceShared\             <- API contracts repo (merabza/CrawlerServiceShared)
├── SystemTools\                      <- shared libraries repo
└── WebSystemTools\                   <- shared web libraries repo
```

Editing the sibling repos is allowed and normal — they build as part of this solution.

## Build and run

```powershell
dotnet build CrawlerService.slnx        # solution is .slnx format; there is no .sln
dotnet run --project CrawlerService     # dev server, listens on http://*:5028 (Swagger UI in dev)
```

- .NET 10 (`net10.0`), SQL Server, `ImplicitUsings` disabled, `Nullable` enabled.
- There are no test projects; verification = clean build (analyzers are strict, see below).
- **All warnings are errors** (`TreatWarningsAsErrors`, `AnalysisMode=All`, SonarAnalyzer in every project via `Directory.Build.props`). Rules that have actually broken builds here: CA1849 (no sync-over-async in async methods — `CommitAsync()`, not `Commit()`) and CA1873 (wrap log calls with computed arguments in `logger.IsEnabled(...)` checks).
- **MSB3027 (file locked) at the bin-copy stage means the app is running** (the CrawlerService dev server or the CrawlerConsole client). The compile itself succeeded. Either stop the app or verify compile-only with `-p:OutputPath=<scratch dir>`; do not kill the user's running app without asking.
- NuGet versions are centrally managed in `Directory.Packages.props` (`ManagePackageVersionsCentrally`).

## Critical: ProjectReference only for SystemTools/WebSystemTools

**Zero `PackageReference` to `SystemTools.*` or `WebSystemTools.*` may exist anywhere in the solution tree** (all four repos — grep every csproj when in doubt). These are consumed exclusively via `ProjectReference` (`..\..\SystemTools\<name>\<name>.csproj`).

Why: deployment publishes self-contained with a global `/p:AssemblyVersion` stamp applied to every source-built project. A single leftover package edge inserts an unstamped (1.0.0.0) package node into the restore graph; NuGet version unification then evicts the project node, publish ships the 1.0.0.0 DLL while stamped DLLs request the stamped version, and the service dies on Linux at startup with `FileNotFoundException`. To verify, check the app's `obj/project.assets.json`: every `SystemTools.*`/`WebSystemTools.*` entry must be `"type": "project"`.

## Critical: CrawlerServiceShared exists as two clones

`CrawlerServiceShared` is cloned into two solution trees: this one and `D:\1WorkDotnet\CrawlerConsole\CrawlerServiceShared` (the console client's tree). Any contract change (DTOs, routes in `CrawlerServiceApiRoutes`, `CrawlerServiceApiClient`) must land in **both** clones — commit+push in one, pull in the other, or apply identically by hand. One-sided edits drift silently and surface as runtime 500s between console and server.

## Architecture

ASP.NET Core minimal-API service that crawls websites and stores text-analysis results (terms, punctuation statistics, URL graph) in SQL Server — built for Georgian-language content (`CrawlerParameters` in appsettings.json defines the alphabet and punctuation set). The interactive UI is a separate app (CrawlerConsole) that talks to this service over HTTP + SignalR.

Request flow: `CrawlerServiceApi/Endpoints/V1/*` (minimal APIs, mapped by `UseCrawlerApiEndpoints`) → MediatR request from `CommandRequests/` → handler in `Handlers/` → `ICrawlerRepository` → `ICrawlerServiceApplicationDbContext` (implemented by `CrawlerDbContext`).

Long-running crawls: the RunTask/RunBatch/TestOnePage handlers launch a `CrawlerReCounter` (`CrawlerService.Application/ReCounters`) — a background "ReCounter" process (SystemTools.ReCounterAbstraction) that streams progress to clients over SignalR. It drives the crawl engine in `CrawlerService.Application/Crawling`: `CrawlerRunnerToolAction` → `BatchPartRunner` → states (`GetPagesState`, `ParseOnePageState`). `ICrawlProgressReporter` bridges engine progress to SignalR; progress-reporting failures are swallowed by design — they must never abort a crawl.

Project dependency chain (bottom-up):

- `CrawlerServiceRoot.Domain` (sibling repo CrawlerServiceRoot) — EF entities, one folder per entity: Batches/Batch, BatchParts/BatchPart, HostModels/HostModel, SchemeModels/SchemeModel, TaskModels/TaskModel, Terms/Term, TermsByUrls/TermByUrl, UrlModels/UrlModel, UrlGraphNodes/UrlGraphNode, Robots/Robot, …; namespace = `CrawlerServiceRoot.Domain.<Folder>`
- `CrawlerServiceRoot.Application.Abstractions` — `ICrawlerServiceApplicationDbContext` (the 15 DbSets + `SaveChanges`/`BeginTransaction`/`Entry`/`Update`); repositories depend on this interface, never on the concrete context
- `CrawlerServiceDbPart.Db` (sibling repo CrawlerServiceDbPart) — `CrawlerDbContext : DbContext, ICrawlerServiceApplicationDbContext` + per-entity configurations; `CrawlerService.Application` references it only for the `*Configuration.*Length` constants
- `CrawlerService.Application` — the application layer, one folder per concern (namespace = `CrawlerService.Application.<Folder>`):
  - `Repositories/` — `ICrawlerRepository` + its EF implementation `CrawlerRepository`, `ICrawlerRepositoryCreatorFactory` + `CrawlerRepositoryCreatorFactory`
  - `RobotsTxt/` — standalone robots.txt parser (`Robots`, `RobotsFactory`, `UriFactory`, `Entities/`, `Enums/`)
  - `Crawling/` — crawl engine: page fetching (named HttpClient `BatchPartRunner.CrawlerClient`, redirects handled manually, custom User-Agent), HtmlAgilityPack parsing, URL extraction/dedup, term extraction; `Models/`, `States/`, `ToolActions/`
  - `ReCounters/` — `CrawlerReCounter` background crawl wrapper with SignalR progress, `CrawlRequest`, `ECrawlKind`
  - `DependencyInjection/` — `AddCrawlerServiceApplication` registers `CrawlerParameters`, the repositories and the crawler HttpClient
- `CrawlerService.WebApi` — endpoints + MediatR handlers
- `CrawlerService` — the host (`Program.cs`): Serilog, Swagger, API-key auth, Windows-service support; `AddCrawlerServiceDb` registers `CrawlerDbContext` and forwards `ICrawlerServiceApplicationDbContext` to the same scoped instance

Comments in the code are frequently in Georgian — keep them and match the surrounding style.

## EF Core migrations

The migrations assembly (`CrawlerServiceDbTools.DbMigration`) and the EF design-time startup project (`CrawlerServiceDbTools.FakeHost`) live in a **separate repository**: `merabza/CrawlerServiceDbTools`, checked out locally at `D:\1WorkDotnet\CrawlerServiceDbTools\CrawlerServiceDbTools` (its solution tree carries its own clones of CrawlerServiceRoot, CrawlerServiceDbPart and SystemTools). The design-time factory reads the connection string (`ConnectionString`) from that FakeHost's user secrets. There is no auto-migration at startup.

```powershell
# run from D:\1WorkDotnet\CrawlerServiceDbTools\CrawlerServiceDbTools
dotnet ef migrations add <Name> --project CrawlerServiceDbTools.DbMigration --startup-project CrawlerServiceDbTools.FakeHost
dotnet ef database update --project CrawlerServiceDbTools.DbMigration --startup-project CrawlerServiceDbTools.FakeHost
```

## Configuration and deployment

- `appsettings.json` holds structure only; real values (connection string, API keys, `MediatRLicenseKey`) come from user secrets in Development and the deployed appsettings in Production. Every API call requires an API key (`ApiKeys` section, validated per remote IP).
- Production runs on a **Linux** server (self-contained `linux-x64` publish); on Windows the service runs in Development. Watch for Windows-only assumptions: no `Console.ReadKey` on a non-interactive host, and on Unix `new Uri("/relative/path")` parses as an absolute `file://` URI instead of throwing — treat file-scheme results as relative when parsing hrefs (this once silently dropped every internal link on Linux).
- Deployment is done externally by SupportTools (`dotnet publish --self-contained /p:AssemblyVersion=1.0.<stamp>`), and the stamp is verified against the running app's version endpoint — never remove AssemblyVersion stamping.
- Serilog writes to console and `Logs/log-<date>.txt` next to the binary.

## Conventions

- Commit messages are timestamps in `yyyyMMddHHmm` form (e.g. `202607211919`).
