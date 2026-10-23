# Resolver Reach

Resolver Reach is a Windows 11 25H2 x64 desktop tool for checking whether DNS-over-HTTPS (DoH) resolvers complete a valid HTTPS DNS exchange.

## Project continuity records

This repository contains two Windows desktop applications. For persistent cross-session context and a chronological record of substantive work, read these files first:

- [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md) — project inventory, architecture, verified feature requirements, user workflow preferences, release checks, and the required logging procedure.
- [DEVELOPMENT_LOG.md](DEVELOPMENT_LOG.md) — append-only chronological record of decisions, changes, tests, releases, and known limitations.

For every substantive project task, update the development log with what actually changed and what was actually tested. Do not claim a build or release artifact succeeded unless verified. Keep both applications' visible product-version information and Save Logs features intact unless explicitly instructed otherwise.

## Features

- Windows x64 release only; ARM64 is intentionally not targeted.
- Single maximized FluentAvalonia window with selectable system, light, and dark themes, an integrated results view, and a Paste Links menu for custom endpoints.
- Original Resolver Reach app icon: a blue-gradient R mark with DNS circuit nodes on a dark navy field. It uses the visual language of modern creative-app icons without reusing Adobe's Photoshop logo.
- Paste a batch of HTTPS DoH endpoint URLs (one per line or separated with semicolons).
- Sends a real RFC 8484 wire-format DNS query for example.com A; validates HTTP status, application/dns-message content type, DNS transaction ID, response flags, question, and DNS response code. A TLS/host connection alone is not counted as success.
- Shows endpoint status, HTTP status, latency, and DNS response details.
- **Save Logs** opens the native save-file picker and exports the current session summary and endpoint results to a UTF-8 text file.
- **Check the Curl Resolvers** fetches the current public resolver table from the cURL DoH wiki, extracts only Base URL-column endpoints, deduplicates the HTTPS URLs, then tests every listed endpoint with bounded concurrency and a cancellable progress display. The full catalog is refreshed on each run; it is not a baked-in snapshot.
- Reports valid DoH responses separately from DNS RCODE errors and unreachable/invalid endpoints. A successful HTTP request alone does not count as a valid DNS exchange. Resolver availability is a point-in-time measurement, not a guarantee of 100% uptime.
- Supports RFC 8484 POST and retries a GET-form DNS query for endpoints that reject POST with 400, 405, 415, or 501.
- Press Escape to cancel an in-progress scan; results and progress stay in the main window.
- GitHub Actions builds the self-contained x64 application and compiles an Inno Setup installer. Product version (currently 1.0.0) is shown in the main header. The Windows app icon lives at src/ResolverReach/Assets/ResolverReach.ico and is used for the executable, both windows, and installer.

## Build locally on Windows

Install the .NET 10 SDK and Inno Setup 6. From a PowerShell terminal at the repository root:

    dotnet restore src/ResolverReach/ResolverReach.csproj
    dotnet publish src/ResolverReach/ResolverReach.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o artifacts/publish
    iscc /DMyAppVersion=1.0.0 installer/ResolverReach.iss

Click **Check the Curl Resolvers** to refresh resolver URLs from https://github.com/curl/curl/wiki/DNS-over-HTTPS and test the listed HTTPS DNS endpoints. Each resolver receives a test query for example.com; use this only when you are comfortable sending that query to every listed provider. Checks are point-in-time and cannot guarantee future or universal reachability.

The workflow under .github/workflows/windows-build.yml runs on pushes to main and manual dispatch. Download the ResolverReach-Windows-x64 artifact from the run's Artifacts section.

## UI and accessibility approach

- The app uses Avalonia with FluentAvalonia and Microsoft's Fluent Design guidance for layout hierarchy, predictable commands, keyboard access, readable contrast, and light/dark themes.
- Windows x64 manifest includes DPI awareness and long-path support. Endpoint checks use invariant URL/DNS parsing and display meaningful status text rather than relying on color alone.
- See Microsoft's [Windows app design guidelines](https://learn.microsoft.com/en-us/windows/apps/design/guidelines-overview), [accessibility best practices](https://learn.microsoft.com/en-us/windows/apps/get-started/best-practices), and [localization guidance](https://learn.microsoft.com/en-us/windows/apps/design/globalizing/prepare-your-app-for-localization).
- These design choices are not a certification of conformance to every international standard. Formal WCAG, localization, keyboard, assistive-technology, and high-DPI testing remains a release requirement.

Resolver Reach is an independent DNS diagnostics tool, not a VPN or system DNS manager.

## Privacy and safety

Resolver Reach sends DNS-over-HTTPS queries only to the HTTPS URLs in the input box. It does not change Windows' system DNS settings, install a network driver, or route general traffic through the resolver. Avoid endpoints you do not trust; the chosen endpoint receives the test DNS query.

## Companion utility: Ollama GGUF Importer

The repository also includes a separate portable Windows x64 utility for importing local GGUF model files into Ollama. Its main header displays the product version (currently 1.0.0), and its executable/window use the dedicated icon at src/OllamaGgufImporter/Assets/OllamaGgufImporter.ico. It detects the current user's default Ollama model-store path, supports multi-select and common split GGUF shard sets, and registers models through Ollama's supported Modelfile + ollama create process rather than copying files directly into Ollama's internal store.

- Includes a **Save Logs** button that opens a Save As dialog and exports the activity log plus the current import queue.
- Source and instructions: [src/OllamaGgufImporter](src/OllamaGgufImporter/README.md)
- Build artifact: download **Ollama-GGUF-Importer-Windows-x64** from the [GitHub Actions workflow](https://github.com/PCUser2021/ResolverReach/actions/workflows/ollama-gguf-importer.yml).

