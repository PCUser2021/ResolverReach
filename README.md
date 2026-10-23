# Resolver Reach

Resolver Reach **2.0.0** is a Windows 11 25H2 x64 desktop diagnostic tool for checking whether DNS-over-HTTPS (DoH) resolvers complete a valid HTTPS DNS exchange.

## Project continuity records

This repository contains two Windows desktop applications. For persistent context and chronological records, read:

- [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md) — architecture, requirements, release checks, and durable user preferences.
- [DEVELOPMENT_LOG.md](DEVELOPMENT_LOG.md) — decisions, changes, tests, releases, and limitations.
- [PROMPT_ARCHIVE.md](PROMPT_ARCHIVE.md) — private archive of repository-related prompts and task outcomes.

## Resolver Reach 2.0.0

- **One window only.** Inputs, progress, aggregate status, and all endpoint results are displayed in the main application window.
- **Exactly two visible testing paths:** click **Test cURL Resolvers** to fetch and test the current cURL wiki catalog, or paste URLs into the input field and click **Test Resolvers**.
- The layout adopts dark slate panels, rounded cards/rows, a strong red status strip, and restrained pink accents based on the user-provided visual reference.
- The live catalog is refreshed on each cURL run. The app extracts unique HTTPS URLs from the wiki's **Base URL** column rather than treating provider homepages as resolvers.
- Pasted URLs can be separated by line breaks or semicolons.
- Sends an RFC 8484 wire-format DNS query for `example.com A`; validates HTTP status, `application/dns-message` content type, DNS transaction ID, response flags, question, and DNS response code. HTTP/TLS success alone does not count as a valid DoH exchange.
- Displays endpoint status, HTTP status, latency, and DNS response details in the main window. PASS indicates a valid NOERROR reply; DNS ERROR means a valid exchange returned a DNS error code; FAIL indicates an unavailable endpoint or invalid response.
- Uses bounded concurrency and a maximum of 512 unique endpoints per run. Press **Escape** to cancel an active scan. It does not change Windows DNS settings or route general traffic.
- Product version 2.0.0 appears in the header. The UI intentionally has no theme picker, menu, log-save dialog, or secondary result window.

### Build locally on Windows

Install the .NET 10 SDK and Inno Setup 6. From a PowerShell terminal at the repository root:

```powershell
dotnet restore src/ResolverReach/ResolverReach.csproj
dotnet publish src/ResolverReach/ResolverReach.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o artifacts/publish
iscc /DMyAppVersion=2.0.0 installer/ResolverReach.iss
```

The workflow under [GitHub Actions](https://github.com/PCUser2021/ResolverReach/actions/workflows/windows-build.yml) builds the self-contained Windows x64 application and installer. Download `ResolverReach-Windows-x64` from a successful run.

Resolver availability is a point-in-time measurement. Each chosen endpoint receives a DNS query for `example.com`; test only providers you trust. Resolver Reach is a diagnostic utility, not a VPN or system DNS manager.

## Companion utility: Ollama GGUF Importer

The repository also includes a separate portable Windows x64 utility for importing local GGUF model files into Ollama. Its main header displays product version 1.0.0, and its executable/window use the dedicated icon at `src/OllamaGgufImporter/Assets/OllamaGgufImporter.ico`. It detects the default Ollama model-store path, supports multi-select and common split GGUF shard sets, and registers models through Ollama's supported temporary Modelfile + `ollama create` process rather than copying files into Ollama's internal store.

- Includes **Save Logs**, exporting the activity log and current import queue.
- Source and instructions: [Ollama GGUF Importer README](src/OllamaGgufImporter/README.md)
- Build artifact: [Ollama-GGUF-Importer-Windows-x64 workflow](https://github.com/PCUser2021/ResolverReach/actions/workflows/ollama-gguf-importer.yml)
