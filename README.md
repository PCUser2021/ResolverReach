<p align="center">
  <img src="public-distribution/assets/resolverreach-icon.svg" alt="Resolver Reach app icon" width="108" />
</p>

<h1 align="center">Resolver Reach 2.1.1</h1>

<p align="center"><strong>Windows 11 x64 · DNS-over-HTTPS reachability testing · One main window</strong></p>

<p align="center">
  <a href="https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe"><strong>⬇ Download the latest Windows x64 installer</strong></a>
  · <a href="https://github.com/PCUser2021/ResolverReach/releases">All releases</a>
  · <a href="LICENSE">MIT License</a>
</p>

<p align="center">
  <img src="public-distribution/assets/resolverreach-preview.svg" alt="Resolver Reach 2.0.0 interface design preview" width="100%" />
</p>

*Image note: the graphic above is an illustrative UI design preview based on the v2.0.0 layout, not a screenshot captured from a running application.*

Resolver Reach is a Windows desktop tool for checking whether DNS-over-HTTPS (DoH) resolvers complete a valid DNS exchange. It provides two testing paths, while progress, endpoint status, latency, and DNS response details remain in the same main window.

## What Resolver Reach does

- **Test cURL Resolvers:** fetch the live [cURL DNS-over-HTTPS resolver catalog](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extract unique HTTPS endpoints from its Base URL column, and test the current list.
- **Paste links → Test Resolvers:** test your own HTTPS DoH endpoints. Paste one URL per line or separate URLs with semicolons.
- **Validate actual DoH responses:** send an RFC 8484 DNS query for `example.com A` and validate HTTP status, `application/dns-message` content type, DNS transaction ID, response flags, echoed question, and DNS response code. HTTP/TLS success alone is not a successful DNS test.
- **Integrated results:** view PASS, DNS ERROR, or FAIL with HTTP status, latency, DNS response details, and scan progress.
- **Bounded scanning:** up to 512 unique endpoints per run; Escape cancels an active scan.
- **More resilient probing:** longer connection tolerance, one retry for request timeouts and transient HTTP errors, and HTTP/1.1 fallback after HTTP/2 protocol resets.
- **Clearer diagnosis:** separates transport failures from endpoints that returned an HTTP response but failed DoH validation.
- **Save Logs:** choose a destination and export a ZIP containing the complete current-session activity log, all endpoint outcomes across scans in the session, currently displayed results, a session summary, and JSON runtime/session metadata.

A valid DNS error response is distinguished from an unavailable endpoint or invalid reply. Availability checks are point-in-time measurements, not a guarantee of future reachability. The chosen DNS provider receives the test query; use services you trust.

## Download

- **[Download the latest Windows x64 installer](https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe)** — stable URL for the newest published GitHub Release.
- **[View all releases and assets](https://github.com/PCUser2021/ResolverReach/releases)**.
- Public downloads require the repository to be public; private repositories' release assets remain access-controlled.

**Current published release:** [Resolver Reach 2.1.1](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.0). The verified Windows build and release workflow ([run #39937042568](https://github.com/PCUser2021/ResolverReach/actions/runs/39937042568)) uploaded `ResolverReach-Setup-x64.exe`. Future tagged product releases are published by the release job; short-lived Actions artifacts remain as a secondary fallback.

## License

Resolver Reach's original project source, documentation, and original assets in this repository are licensed under the permissive **MIT License**. Anyone may use, copy, modify, merge, publish, distribute, sublicense, and sell copies for any purpose, subject to preserving the copyright and license notice.

See [LICENSE](LICENSE). Third-party libraries, dependencies, and trademarks remain subject to their own licenses and terms.

## Build verification

Resolver Reach 2.1.1 is a Windows 11 x64 app built with C# / .NET 10, Avalonia, FluentAvalonia, ReactiveUI, and DynamicData. The v2.0.0 Windows x64 publish and installer compile passed in [GitHub Actions run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989). CI packaging success does not mean every DNS resolver has been live-tested.

## Technical details

- Version: **2.1.1**
- Target: Windows 11 x64
- DoH test: RFC 8484 DNS wire-format query with POST and GET fallback where appropriate
- No changes to Windows DNS settings, VPN, or general traffic routing


## JDownloader Reach — integrated download and tools shell

The Windows x64 and ARM64 prototype is being developed on the isolated branch `feature/jdownloader-reach-prototype`. Version 0.2.0 adds an Avalonia shell that can communicate with an existing local JDownloader engine (add links to LinkGrabber, inspect a basic package snapshot, and start queued downloads) and launch both repository applications: Resolver Reach and Ollama GGUF Importer.

- [Prototype documentation](src/JDownloaderReach/README.md)
- [Integration architecture and phases](docs/JDownloaderReach-Integration-Architecture.md)
- [Prototype change log](src/JDownloaderReach/CHANGELOG.md)

**Important:** This is not a complete Java-to-C# conversion and does not bundle the JDownloader engine. The x64 and ARM64 installers are built independently and each installer is restricted to its matching OS architecture. The two architecture-specific installers package native builds of all three .NET applications; JDownloader must be installed separately. The optional deprecated local API is restricted in this client to `127.0.0.1:3128` and must not be exposed to a network.
