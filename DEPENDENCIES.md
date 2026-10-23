# Resolver Reach — stack, dependencies, and build toolchain

**Audit date:** 2026-10-23  
**Release candidate:** 2.4.0 (cross-platform CI/release pending)  
**Last release recorded before this change:** v2.2.0 (until v2.4.0 assets are verified and published)  
**Repository:** https://github.com/PCUser2021/ResolverReach

This file records the package and toolchain versions declared by the repository and the versions selected for the new test project. It distinguishes pinned versions from tools installed dynamically by CI; an unpinned installer or runner must not be described as an exact version.

## Application stack

| Component | Version used | Purpose |
|---|---:|---|
| C# language/compiler | C# 14 via .NET 10 SDK | Application and tests |
| .NET target framework | `net10.0` | Application and test target |
| .NET SDK selection | `global.json`: `10.0.402`, `rollForward: latestFeature`; setup-dotnet requests pinned `10.0.402` | Build workflows are pinned to the latest stable SDK verified on 2026-10-23; global.json enables the .NET 10 Microsoft Testing Platform runner |
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
| coverlet.MTP | `10.1.0` | Coverlet integration for Microsoft Testing Platform; JSON/Cobertura reports |
| Microsoft.Testing.Extensions.TrxReport | `2.5.1` | Produces machine-readable TRX test reports in CI |
| Test runner | Microsoft Testing Platform via `global.json` | The .NET 10 SDK runner is explicitly selected; MTP version is transitive through xunit.v3, not separately pinned in this repository |
| Test target | `net10.0` | Offline/deterministic protocol-unit tests |

Test fixtures are synthetic DNS packets: the protocol tests do not depend on public resolver availability or make live DNS requests. The CI workflow runs tests separately from the architecture-specific publish jobs.

## Build, packaging and CI

| Tool | Repository setting | Versioning note |
|---|---|---|
| GitHub Actions runner | `windows-latest` for tests and Windows builds | Verified CI run #40001573171 used runner `2.338.0`, image `20261021.618`, image version `20261021.278.1`; these are rolling, not immutable pins |
| GitHub Actions runner | `ubuntu-latest` for artifact round-trip verification | Verified CI run #40001573171 used runner `2.338.0`, image `20261012.601`, image version `20261012.334.1`; these are rolling, not immutable pins |
| actions/checkout | `@v7`; resolved SHA in run #40001573171: `3d3c42e5aac5ba805825da76410c181273ba90b1` | Workflow uses major tag, not a full commit-SHA pin |
| actions/setup-dotnet | `@v6`; resolved SHA in run #40001573171: `a98b56852c35b8e3190ac28c8c2271da59106c68` | Workflow uses major tag; installs `.NET 10.0.x` |
| actions/upload-artifact | `@v7`; resolved SHA in run #40001573171: `cf430e030ddbb5b0abf93d22962f4752f3646cd9` | Workflow uses major tag, not a full commit-SHA pin |
| actions/download-artifact | `@v8`; resolved SHA in run #40001573171: `9000827ccba6bdab643e8b6fd33ac0654aef8333` | Workflow uses major tag, not a full commit-SHA pin |
| softprops/action-gh-release | `@v3.0.3` | Explicit version tag |
| Inno Setup compiler | **6.7.1 in verified CI run #40001573171** | Workflow checks Inno Setup 7 paths first, then 6; current installer was built with Inno Setup 6.7.1. The workflow does not pin that patch version, so future runners could select a different release |
| Chocolatey install fallback | `choco install innosetup --yes --no-progress` | This fallback does not pin an installer-package version; the verified run found Inno Setup 6.7.1. Exact future installation depends on runner/package feed |
| Windows packaging | Inno Setup script `installer/ResolverReach.iss` | Produces separate x64-compatible and native ARM64 installers |
| macOS packaging | Native `hdiutil` + `ditto` on GitHub-hosted macOS runner | Produces DMG and ZIP containing `Resolver Reach.app` for `osx-x64` and `osx-arm64`; app bundle metadata sets minimum macOS 15.0 |
| macOS signing/notarization | Not configured | No Apple Developer signing credentials are available to this workflow; packages are unsigned and not notarized, so Gatekeeper may block first launch |
| Git in Windows CI | `2.56.0.windows.2` | Observed in both architecture-build/test job logs for run #40001573171; not part of the shipped app |
| Publish model | .NET self-contained single-file | `win-x64` and `win-arm64`; native runtime bits are embedded/extracted by .NET publish configuration |

## Current stable-version audit (2026-10-23)

The workflow uses .NET SDK 10.0.402 and .NET 10 runtime servicing release 10.0.13, both published 2026-10-13. NuGet stable listings checked for this update show Avalonia/Avalonia.Desktop 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6, ReactiveUI.Reactive 26.0.1, DynamicData 9.5.0, System.Reactive 7.0.0, xunit.v3 4.0.2, and the existing test/coverage package versions as current stable releases. Do not upgrade packages to prereleases merely because their version number is higher.

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
