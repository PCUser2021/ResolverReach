# Resolver Reach

Resolver Reach is a Windows 11 25H2 x64 desktop tool for checking whether DNS-over-HTTPS (DoH) resolvers complete a valid HTTPS DNS exchange.

## Features

- Windows x64 release only; ARM64 is intentionally not targeted.
- Compact, dark FluentAvalonia interface inspired by OnionFruit's small status-window layout and Photoshop's high-contrast blue-on-navy creative-software palette.
- Original Resolver Reach app icon: a blue-gradient R mark with DNS circuit nodes on a dark navy field. It uses the visual language of modern creative-app icons without reusing Adobe's Photoshop logo.
- Paste a batch of HTTPS DoH endpoint URLs (one per line or separated with semicolons).
- Sends a real RFC 8484 wire-format DNS query for example.com A; validates HTTP status, application/dns-message content type, DNS transaction ID, response flags, question, and DNS response code. A TLS/host connection alone is not counted as success.
- Shows endpoint status, HTTP status, latency, and DNS response details.
- **Check Now** fetches the current public resolver table from the cURL DoH wiki, extracts only Base URL-column endpoints, deduplicates the HTTPS URLs, then tests every listed endpoint with bounded concurrency and a cancellable progress display. The full catalog is refreshed on each run; it is not a baked-in snapshot.
- Reports valid DoH responses separately from DNS RCODE errors and unreachable/invalid endpoints. A successful HTTP request alone does not count as a valid DNS exchange. Resolver availability is a point-in-time measurement, not a guarantee of 100% uptime.
- Supports RFC 8484 POST and retries a GET-form DNS query for endpoints that reject POST with 400, 405, 415, or 501.
- Exports results to CSV.
- GitHub Actions builds the self-contained x64 application and compiles an Inno Setup installer. The Windows app icon lives at src/ResolverReach/Assets/ResolverReach.ico and is used for the executable, both windows, and installer.

## Build locally on Windows

Install the .NET 10 SDK and Inno Setup 6. From a PowerShell terminal at the repository root:

    dotnet restore src/ResolverReach/ResolverReach.csproj
    dotnet publish src/ResolverReach/ResolverReach.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o artifacts/publish
    iscc /DMyAppVersion=1.0.0 installer/ResolverReach.iss

Click **Check Now** to refresh resolver URLs from https://github.com/curl/curl/wiki/DNS-over-HTTPS and test the listed HTTPS DNS endpoints. Each resolver receives a test query for example.com; use this only when you are comfortable sending that query to every listed provider. Checks are point-in-time and cannot guarantee future or universal reachability.

The workflow under .github/workflows/windows-build.yml runs on pushes to main and manual dispatch. Download the ResolverReach-Windows-x64 artifact from the run's Artifacts section.

## UI/framework references

- OnionFruit product page and public source: https://dragonfruit.network/onionfruit and https://github.com/dragonfruitnetwork/onionfruit/tree/2026.620.0. Resolver Reach borrows only high-level visual cues such as the compact status ribbon and acrylic window surface.
- Photoshop reference: https://en.wikipedia.org/wiki/Adobe_Photoshop. The icon and theme are independently designed around a dark field and vivid blue accents; they do not use Adobe's Photoshop logo or name in the app identity.

Resolver Reach is an independent DNS diagnostics tool, not an OnionFruit fork, VPN, or Tor client. See THIRD-PARTY-NOTICES.md.

## Privacy and safety

Resolver Reach sends DNS-over-HTTPS queries only to the HTTPS URLs in the input box. It does not change Windows' system DNS settings, install a network driver, or route general traffic through the resolver. Avoid endpoints you do not trust; the chosen endpoint receives the test DNS query.

## Companion utility: Ollama GGUF Importer

The repository also includes a separate portable Windows x64 utility for importing local GGUF model files into Ollama. It detects the current user's default Ollama model-store path, supports multi-select and common split GGUF shard sets, and registers models through Ollama's supported Modelfile + ollama create process rather than copying files directly into Ollama's internal store.

- Source and instructions: [src/OllamaGgufImporter](src/OllamaGgufImporter/README.md)
- Build artifact: download **Ollama-GGUF-Importer-Windows-x64** from the [GitHub Actions workflow](https://github.com/PCUser2021/ResolverReach/actions/workflows/ollama-gguf-importer.yml).
