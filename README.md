<p align="center">
  <img src="public-distribution/assets/resolverreach-icon.svg" alt="Resolver Reach app icon" width="112" />
</p>

<h1 align="center">Resolver Reach 2.2.0</h1>

<p align="center"><strong>Windows 11 x64 and native ARM64 · DNS-over-HTTPS resolver diagnostics</strong></p>

<p align="center">
  <a href="https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe"><strong>Download Windows x64</strong></a>
  · <a href="https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe"><strong>Download native ARM64</strong></a>
  · <a href="https://github.com/PCUser2021/ResolverReach/releases">All releases</a>
</p>

<p align="center">
  <img src="public-distribution/assets/resolverreach-preview.svg" alt="Resolver Reach 2.2.0 interface design preview" width="100%" />
</p>

*Image note: the graphic above is an illustrative UI design preview based on the current layout, not a screenshot captured from a running application.*

Resolver Reach checks whether DNS-over-HTTPS (DoH) endpoints complete a valid DNS exchange. It tests the live cURL DoH catalog or your pasted HTTPS endpoints, displays diagnostic results in one main window, and exports the current session's full diagnostic history to a ZIP archive.

## Features

- **Test cURL Resolvers:** fetch the live [cURL DNS-over-HTTPS resolver catalog](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extract unique HTTPS Base URLs, and test the current list.
- **Paste links → Test Resolvers:** test your own HTTPS endpoints, one URL per line or separated with semicolons.
- **Validate real DNS exchanges:** send RFC 8484 DNS requests and validate HTTP status, the `application/dns-message` content type, DNS transaction ID, response flags, echoed question, and response code. HTTP/TLS success alone is not a successful DNS test.
- **Clear failure accounting:** distinguishes transport failures from HTTP responses that fail DoH validation; DNS error codes are reported separately from invalid/unreachable replies.
- **Resilient probe handling:** longer connection tolerance, a bounded retry for timeouts/transient HTTP status codes, and HTTP/1.1 compatibility fallback after HTTP/2 protocol errors.
- **Save Logs to ZIP:** choose the destination and export session summary, activity log, endpoint result history, currently visible results, and session/runtime metadata.
- **Dual Windows architecture:** separate self-contained `win-x64` and native `win-arm64` applications/installers.
- **Original vector icon family:** dark titanium/glass forms with spectral cyan, blue, violet, and magenta accents, applied consistently to the app mark, scan/paste/log actions, status states, and empty state.

A valid DNS error response is a completed DoH exchange but is not a successful answer. Dead hostnames, access-denied endpoints, non-DNS content, and invalid DNS replies remain failures. Checks are point-in-time; no client can guarantee every third-party resolver will be available.

## Downloads

**Current release:** [Resolver Reach 2.2.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0). The x64 and native ARM64 builds, installers, and Release upload all passed in [GitHub Actions run #39945785064](https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064).

- **[Windows x64 installer](https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe)** — standard Intel/AMD 64-bit Windows. The x64-compatible installer can also run under Windows 11 ARM64 emulation.
- **[Native Windows ARM64 installer](https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe)** — native Arm64 Windows app, not the emulated x64 build.
- [Browse all releases and checksums](https://github.com/PCUser2021/ResolverReach/releases).

Public anonymous downloads require the repository to be public; private release assets remain access-controlled.

## License

Original Resolver Reach source, documentation, and project-authored assets are licensed under the **MIT License**. Anyone may use, copy, modify, publish, distribute, sublicense, and sell copies for any purpose, subject to retaining the copyright and license notice. Third-party libraries and other third-party material remain under their respective licenses.

See [LICENSE](LICENSE).

## Technical details

- Version: **2.2.0**
- Runtime identifiers: `win-x64`, `win-arm64`
- Stack: C# / .NET 10, Avalonia, FluentAvalonia, ReactiveUI, DynamicData
- DNS check: RFC 8484 wire-format request and response validation
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
