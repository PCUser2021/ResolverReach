# Resolver Reach

Resolver Reach is a Windows 11 25H2 x64 desktop tool for checking whether DNS-over-HTTPS (DoH) resolvers complete a valid HTTPS DNS exchange.

## Features

- Windows x64 release only; ARM64 is intentionally not targeted.
- Light UI built with Avalonia and FluentAvalonia styling.
- Paste a batch of HTTPS DoH endpoint URLs (one per line or separated with semicolons).
- Sends a real RFC 8484 wire-format DNS query for example.com A; validates HTTP status, application/dns-message content type, DNS transaction ID, response flags, question, and DNS response code. A TLS/host connection alone is not counted as success.
- Shows endpoint status, HTTP status, latency, and DNS response details.
- Exports results to CSV.
- GitHub Actions builds the self-contained x64 application and compiles an Inno Setup installer.

## Build locally on Windows

Install the .NET 10 SDK and Inno Setup 6. From a PowerShell terminal at the repository root:

    dotnet restore src/ResolverReach/ResolverReach.csproj
    dotnet publish src/ResolverReach/ResolverReach.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o artifacts/publish
    iscc /DMyAppVersion=1.0.0 installer/ResolverReach.iss

The workflow under .github/workflows/windows-build.yml runs on pushes to main and manual dispatch. Download the ResolverReach-Windows-x64 artifact from the run's Artifacts section.

## UI/framework reference

The look-and-feel and framework choice were informed by the public OnionFruit 2026.620.0 source: https://github.com/dragonfruitnetwork/onionfruit/tree/2026.620.0. Resolver Reach is an independent DNS diagnostics tool, not an OnionFruit fork, VPN, or Tor client. No OnionFruit source files are intentionally copied into this repository. See THIRD-PARTY-NOTICES.md.

## Privacy and safety

Resolver Reach sends DNS-over-HTTPS queries only to the HTTPS URLs in the input box. It does not change Windows' system DNS settings, install a network driver, or route general traffic through the resolver. Avoid endpoints you do not trust; the chosen endpoint receives the test DNS query.
