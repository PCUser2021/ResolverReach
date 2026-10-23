# Resolver Reach — stack, dependencies, and build toolchain

**Audit date:** 2026-10-23  
**Source candidate:** 2.4.0 (not yet published as a GitHub Release)  
**Last release recorded before this change:** v2.2.0  
**Repository:** https://github.com/PCUser2021/ResolverReach

This file records the package and toolchain versions declared by the repository and the versions selected for the new test project. It distinguishes pinned versions from tools installed dynamically by CI; an unpinned installer or runner must not be described as an exact version.

## Application stack

| Component | Version used | Purpose |
|---|---:|---|
| C# language/compiler | C# 14 via .NET 10 SDK | Application and tests |
| .NET target framework | `net10.0` | Application and test target |
| .NET SDK requested by CI | `10.0.x` | CI intentionally follows the latest .NET 10 servicing SDK |
| .NET SDK latest official release at audit time | `10.0.402` (released 2026-10-13) | Current reference for the unpinned `10.0.x` selector |
| .NET 10 runtime servicing release | `10.0.13` (released 2026-10-13) | Current .NET 10 patch line; publish remains self-contained |
| Avalonia | `12.1.4` | Cross-platform .NET UI framework |
| Avalonia.Desktop | `12.1.4` | Desktop platform host |
| FluentAvaloniaUI | `3.1.0` | Fluent-style window and controls |
| ReactiveUI.Avalonia.Reactive | `12.1.6` | Avalonia/ReactiveUI integration package |
| ReactiveUI.Reactive | `26.0.1` | ReactiveUI package compatible with System.Reactive |
| System.Reactive | `7.0.0` | Observable streams, subjects and reactive scheduling |
| DynamicData | `9.5.0` | Observable collection/cache transforms and result sorting |
| `HttpClient` / `SocketsHttpHandler` | .NET 10 BCL | HTTPS DoH transport, HTTP version negotiation, timeouts and connection pooling |
| DNS wire-format code | in-repository C# | RFC 8484 request/response validation; no external DNS parsing library |
| cURL public resolver catalog | live cURL wiki Markdown | Runtime data source; not a NuGet dependency |

## Test stack

| Component | Version | Purpose |
|---|---:|---|
| Microsoft.NET.Test.Sdk | `18.11.0` | .NET test discovery and execution |
| xunit.v3 | `4.0.2` | Current xUnit.net v3 test framework |
| xunit.runner.visualstudio | `4.0.1` | Visual Studio/VSTest adapter |
| coverlet.collector | `10.1.0` | Optional Cobertura code-coverage collection in CI |
| Test target | `net10.0` | Offline/deterministic protocol-unit tests |

Test fixtures are synthetic DNS packets: the protocol tests do not depend on public resolver availability or make live DNS requests. The CI workflow runs tests separately from the architecture-specific publish jobs.

## Build, packaging and CI

| Tool | Repository setting | Versioning note |
|---|---|---|
| GitHub Actions runner | `windows-latest` for tests and Windows builds | Hosted runner image is rolling, not pinned to an immutable image |
| GitHub Actions runner | `ubuntu-latest` for artifact round-trip verification | Hosted runner image is rolling |
| actions/checkout | `@v7` | Major tag, not a full commit-SHA pin |
| actions/setup-dotnet | `@v6` | Major tag; installs `.NET 10.0.x` |
| actions/upload-artifact | `@v7` | Major tag, not a full commit-SHA pin |
| actions/download-artifact | `@v8` | Major tag, not a full commit-SHA pin |
| softprops/action-gh-release | `@v3.0.3` | Explicit version tag |
| Inno Setup compiler | Discovery tries Inno Setup 7 then 6 | Exact patch version is not pinned |
| Chocolatey install fallback | `choco install innosetup --yes --no-progress` | Package version is intentionally unpinned; exact installed patch depends on the runner/package feed on that day |
| Windows packaging | Inno Setup script `installer/ResolverReach.iss` | Produces separate x64-compatible and native ARM64 installers |
| Publish model | .NET self-contained single-file | `win-x64` and `win-arm64`; native runtime bits are embedded/extracted by .NET publish configuration |

## Protocol and external standards

- **RFC 8484** — DNS over HTTPS; raw DNS wire bytes on POST, Base64URL `dns` parameter for GET, media type `application/dns-message`.
- **RFC 1035** — DNS message format, name compression, resource-record layout.
- **RFC 6891** — EDNS(0), OPT pseudo-resource record and extended response code.
- **cURL DoH resolver wiki** — runtime catalog source at https://github.com/curl/curl/wiki/DNS-over-HTTPS.

## Deliberate scope choices

- No separate logging framework: session/activity/result logs are already implemented in the application.
- No dependency-injection/hosting framework: this is a single-window desktop utility with a small, static transport/catalog surface; adding hosting abstractions would add dependency weight without a concrete benefit yet.
- No external DNS protocol/parser package: the application needs a narrow, controlled probe, so the parser is kept in-repository and covered by deterministic unit fixtures.
- No external Tor/proxy/VPN engine; Resolver Reach tests DoH endpoints and does not reroute system traffic.
- No external image/asset-generation runtime dependency; application artwork is stored in the repository.
- NuGet transitive dependencies are resolved by NuGet at restore time. The table above lists direct package references maintained by this project, not every transitive binary. For the exact resolved graph, inspect the CI restore assets `obj/project.assets.json` or run `dotnet list package --include-transitive` on the candidate commit.
