<p align="center">
  <img src="public-distribution/assets/resolverreach-icon.svg" alt="Resolver Reach app icon" width="112" />
</p>

<h1 align="center">Resolver Reach</h1>

<p align="center"><strong>Windows 11 x64 and native ARM64 · DNS-over-HTTPS resolver diagnostics</strong></p>

<p align="center">
  <a href="https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe"><strong>Download Windows x64</strong></a>
  · <a href="https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe"><strong>Download native ARM64</strong></a>
  · <a href="https://github.com/PCUser2021/ResolverReach/releases">All releases</a>
</p>

<p align="center">
  <img src="public-distribution/assets/resolverreach-preview.svg" alt="Resolver Reach interface design preview" width="100%" />
</p>

*Image note: the graphic above is an illustrative UI design preview based on the current layout, not a screenshot captured from a running application.*

Resolver Reach checks whether DNS-over-HTTPS (DoH) endpoints complete a valid DNS exchange. It tests the live cURL DoH catalog or your pasted HTTPS endpoints, displays diagnostic results in one main window, and exports the current session's full diagnostic history to a ZIP archive.

## Features

- **Test cURL Resolvers:** fetch the live [cURL DNS-over-HTTPS resolver catalog](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extract unique HTTPS Base URLs, and test the current list.
- **Paste links → Test Resolvers:** test your own HTTPS endpoints, one URL per line or separated with semicolons.
- **Validate real DNS exchanges:** send RFC 8484 DNS requests and validate HTTP status, the `application/dns-message` content type, DNS transaction ID, response flags, echoed question, declared answer/authority/additional record boundaries, common structured RDATA layouts, EDNS OPT/options and extended RCODE. Oversized bodies are rejected while streaming rather than after buffering the full response. HTTP/TLS success alone is not a successful DNS test.
- **Clear failure accounting:** distinguishes transport failures from HTTP responses that fail DoH validation; DNS error codes are reported separately from invalid/unreachable replies. Diagnostics explain observed status/transport classes and distinguish possible filtering from proven censorship.
- **Resilient probe handling:** bounded response reads, longer connection tolerance, a bounded retry for timeouts/transient HTTP status codes, and HTTP/1.1 compatibility fallback after HTTP/2 protocol errors.
- **Provider country and capability profiles:** recognized resolver domains show provider/operator country and flag plus a curated feature summary. Unknown endpoints and unverified DNSSEC capabilities remain explicitly unknown; country does not guarantee anycast egress location.
- **Automated protocol regression tests:** deterministic offline DNS fixtures cover valid NOERROR/NXDOMAIN responses, all response sections, EDNS extended RCODEs, mismatched IDs/questions, malformed/truncated records, invalid compression pointers, and trailing bytes.
- **Save Logs to ZIP:** choose the destination and export session summary, activity log, endpoint result history, currently visible results, and session/runtime metadata.
- **Dual Windows architecture:** separate self-contained `win-x64` and native `win-arm64` applications/installers.
- **Windows 11 Fluent-inspired interface:** official Fluent System Icon geometry is used for key in-window actions/status indicators with a dark surface and blue accent palette; the existing panel/window structure is retained.

A valid DNS error response is a completed DoH exchange but is not a successful answer. Dead hostnames, access-denied endpoints, non-DNS content, and invalid DNS replies remain failures. Checks are point-in-time; no client can guarantee every third-party resolver will be available.

## Downloads

**Latest published release:** Resolver Reach 2.4.0 for Windows. Version 2.5.0 has been merged to `main`; its installer release has not been published yet.

- **[Windows x64 installer](https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe)** — standard Intel/AMD 64-bit Windows. The x64-compatible installer can also run under Windows 11 ARM64 emulation.
- **[Native Windows ARM64 installer](https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe)** — native Arm64 Windows app, not the emulated x64 build.
- [Browse all releases and checksums](https://github.com/PCUser2021/ResolverReach/releases).


Public anonymous downloads require the repository to be public; private release assets remain access-controlled.

## License

Original Resolver Reach source, documentation, and project-authored assets are licensed under the **MIT License**. Anyone may use, copy, modify, publish, distribute, sublicense, and sell copies for any purpose, subject to retaining the copyright and license notice. Third-party libraries and other third-party material remain under their respective licenses.

See [LICENSE](LICENSE).

## Technical details

- Current main version: **2.5.0** (merged; not yet released). Latest published GitHub Release remains v2.4.0.
- Runtime identifiers: `win-x64`, `win-arm64`
- Stack: C# 14 / .NET 10, Avalonia 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6, ReactiveUI.Reactive 26.0.1, System.Reactive 7.0.0, DynamicData 9.5.0; versions are centrally managed in `Directory.Packages.props`
- DNS check: RFC 8484 wire-format request and full declared-section/EDNS boundary validation
- Tests: xUnit.net v3 4.0.2, Microsoft.NET.Test.Sdk 18.11.0, Visual Studio adapter 4.0.1, Microsoft.Testing.Extensions.TrxReport 2.5.1, Coverlet.MTP 10.1.0; `.NET 10` `global.json` selects Microsoft Testing Platform
- Complete packages and build-tool versions: [DEPENDENCIES.md](DEPENDENCIES.md)
- No changes to Windows DNS settings, VPN, or general traffic routing


## Project handoff and durable context

For continuing work across sessions, read:
- [Software project master context](Software_Project_Master_Context.md)
- [Full AI session handoff](AI_SESSION_HANDOFF.md)
- [Project implementation context](PROJECT_CONTEXT.md)
- [Prompt and outcome archive](PROMPT_ARCHIVE.md)
- [Development log](DEVELOPMENT_LOG.md)
- [Backup manifest](BACKUP_MANIFEST.md)

The repository and its prompt archive preserve project-specific context, source, assets, release history and decisions. They are not a complete export of account-level ChatGPT memory or unrelated conversations.
