# CrawlerService

An ASP.NET Core web service that crawls websites and stores text-analysis results in SQL Server. It was built for collecting and analyzing Georgian-language web content: crawled pages are split into terms using a configurable alphabet and punctuation set, and the results (terms, per-URL term sequences, URL graph, robots.txt data) are persisted for later analysis.

Crawling runs as background processes with live progress streamed to clients over SignalR. The service is controlled through a REST API (Swagger UI available in development) and is consumed by a separate console client ([CrawlerConsole](https://github.com/merabza/CrawlerConsole)).

## Features

- Crawl configuration via **schemes**, **hosts**, **batches**, **tasks** and **start points**, managed over the API
- Batch crawling with URL-graph construction and deduplication
- robots.txt parsing and rule evaluation (`RobotsTxt` project)
- Term extraction driven by a configurable alphabet and punctuation table (`CrawlerParameters`)
- Long-running crawls as background "ReCounter" processes with SignalR progress reporting
- API-key authentication (per key, optionally bound to a remote IP)
- Serilog logging to console and rolling files (`Logs/log-<date>.txt`)
- Runs as a Windows service on Windows or a systemd service on Linux

## Repository layout

The solution (`CrawlerService.slnx`) spans four sibling repositories that must be cloned next to each other:

```
<root>\
├── CrawlerService\          this repository
├── CrawlerServiceShared\    shared API contracts and client   (merabza/CrawlerServiceShared)
├── SystemTools\             shared libraries                  (merabza/SystemTools)
└── WebSystemTools\          shared web libraries              (merabza/WebSystemTools)
```

Projects in this repository:

| Project | Purpose |
|---|---|
| `CrawlerService` | Web host: Serilog, Swagger, API-key auth, DI wiring |
| `CrawlerServiceApi` | Minimal-API endpoints (V1) and MediatR command/query handlers |
| `CrawlerServiceReCounters` | Background crawl process with SignalR progress |
| `DoCrawler` | The crawl engine: fetching, HTML parsing, URL and term extraction |
| `RobotsTxt` | robots.txt parser |
| `CrawlerDbModels` | EF Core entities |
| `CrawlerDbPersistence` | `CrawlerDbContext` and entity configurations |
| `CrawlerDbMigration` | EF Core migrations |
| `CrawlerRepoInterfaces` / `LibCrawlerRepositories` | Repository abstraction and implementation |
| `FakeHost` | Design-time host for `dotnet ef` only; not deployed |

## Requirements

- .NET 10 SDK
- SQL Server

## Getting started

1. Clone the four repositories side by side (see layout above).

2. Build:

   ```powershell
   dotnet build CrawlerService.slnx
   ```

3. Configure secrets. `appsettings.json` contains placeholders only; supply real values via user secrets (or environment-specific appsettings):

   - `CrawlerService` project: `Data:CrawlerServiceDatabase:ConnectionString`, `ApiKeys:AppSettingsByApiKey`, `MediatRLicenseKey`
   - `FakeHost` project: `ConnectionStringSeed` (used only by `dotnet ef`)

4. Create the database:

   ```powershell
   dotnet ef database update --project CrawlerDbMigration --startup-project FakeHost
   ```

5. Run:

   ```powershell
   dotnet run --project CrawlerService
   ```

   The service listens on `http://*:5028` by default (see `Kestrel` section in appsettings.json). In development, Swagger UI documents the API; all endpoints require an API key.

## API overview

Endpoint groups (all versioned under V1, routes defined in `CrawlerServiceShared.Contracts`):

- **Schemes / Hosts / Batches / Tasks / Start points** — CRUD for crawl configuration
- **Crawler** — run a batch, run a task, test a single page, clear fetched data; progress is streamed over SignalR

## License

[MIT](LICENSE)
