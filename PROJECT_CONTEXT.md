# Project Context and Working Agreement

Last verified: 2026-10-23
Repository: https://github.com/PCUser2021/ResolverReach
Default branch: main
Visibility last reported: private; public access requires a separate repository-settings change.

This file is the current project overview. Current source, Git history, GitHub Actions runs and published release assets are authoritative when summaries differ.

## Current application — Resolver Reach

**Current in-progress Windows-only release:** PR [#4](https://github.com/PCUser2021/ResolverReach/pull/4), branch `feature/2.4.0-release-macos-backup`. Supported targets are Windows x64 and native ARM64 only. The macOS proposal was cancelled at the user's request and has been removed from the active project files and workflow. The initial CI runs failed without readable job logs; do not merge or publish until a fresh Windows-only run passes.

Main now contains Resolver Reach source candidate 2.4.0, merged at [`803e367`](https://github.com/PCUser2021/ResolverReach/commit/803e36738e567b08a12ff3cdbc4de3675d3dd4f7). The candidate's 12 deterministic protocol tests, x64/ARM64 builds, installer builds and artifact verification passed CI run [#40001573171](https://github.com/PCUser2021/ResolverReach/actions/runs/40001573171). The latest published release remains v2.2.0. See `DEPENDENCIES.md` for the package and build-tool inventory.

- Resolver Reach is the sole application maintained in the current source tree.
- Current main source candidate: 2.4.0 (merged; not yet published as a GitHub Release). Latest published GitHub Release: v2.2.0. Do not claim a v2.4.0 release until its release assets are verified.
- Platform: Windows 11 x64 and native ARM64 only; runtime identifiers: `win-x64` and `win-arm64`.
- Stack: C# 14 / .NET 10; Avalonia 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6, ReactiveUI.Reactive 26.0.1, System.Reactive 7.0.0, DynamicData 9.5.0; direct NuGet versions are centralized in `Directory.Packages.props`.
- Main project: `src/ResolverReach/ResolverReach.csproj`.
- Source directory: `src/ResolverReach/`.
- Main view and UI assets: `src/ResolverReach/MainView.axaml`, `src/ResolverReach/Assets/Icons/`, and `src/ResolverReach/Assets/ResolverReach.ico`.
- Installer: `installer/ResolverReach.iss`.
- CI workflow: `.github/workflows/windows-build.yml`.
- Root README: `README.md`.

## Verified functionality

- Single-window desktop UI with two testing paths: test the live cURL DoH catalog or paste custom HTTPS DoH endpoints.
- Sends RFC 8484 wire-format DNS queries and validates HTTP status, `application/dns-message` content type, DNS transaction ID, response flags, echoed question and response code.
- A TLS or HTTP 200 response alone does not count as a successful DNS exchange.
- Provides bounded concurrency, cancellation, retries for selected timeouts/transient HTTP statuses, and HTTP/1.1 fallback after detected HTTP/2 protocol errors.
- Reports PASS, DNS ERROR, FAIL and CANCELLED distinctly.
- Save Logs exports the current process session (summary, activity, result history, visible results and session metadata) to ZIP.
- Resolver Reach is a diagnostic utility, not a VPN or system DNS manager. It must not change Windows DNS/proxy settings or route general traffic.

## Releases and CI

- Previously published release: v2.2.0. The 2.4.0 release workflow builds Windows x64/ARM64 installers only. Do not claim v2.4.0 is published until its Actions run passes and release assets are visible.
- x64 installer: https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-x64.exe
- Native ARM64 installer: https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-arm64.exe
- Verified release workflow run #39945785064: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064.
- The macOS packaging experiment was removed from the active source/workflow at the user's request to avoid extra development cost. Keep Resolver Reach Windows-only unless the user explicitly reopens that requirement.
- CI validates compilation and packaging; it does not replace live resolver testing or an interactive GUI smoke test.

## Diagnostic blocker

- `ResolverReach-Logs-20261023-032704.zip` was inspected: 266 endpoints, 235 PASS, 1 valid DNS ERROR (RCODE REFUSED), 30 FAIL.
- The newer requested logs `ResolverReach-Logs-20261023-040016.zip` and `ResolverReach-Logs-20261023-044828.zip` were not available as actual ZIP files in the referenced inspection sessions. Do not claim they were analyzed.
- To diagnose those exact runs, the user must attach the ZIP bytes. Then inspect per-endpoint details and distinguish app bugs from resolver or network/VPN failures.

## Working preferences and change policy

- User runs Windows 11 Pro 25H2 x64 and prefers GUI/open-and-click instructions. Avoid PowerShell unless necessary or explicitly requested.
- Prefer direct official download links and verify the actual release asset before calling it available.
- Preserve existing Resolver Reach behavior unless the user explicitly asks for removal or replacement.
- For shipped application changes, update the product version and synchronized metadata, run the applicable x64 and ARM64 build/package checks, inspect artifacts, and update the changelog and project records.
- Documentation-only changes do not require an application version bump.
- Record repository-related prompts and outcomes in `PROMPT_ARCHIVE.md`; append dated implementation and test facts to `DEVELOPMENT_LOG.md`. Never claim a build/test passed unless it was actually checked.
- Be exact about what was inspected, built, tested, released and left incomplete.

## Repository map

- Overview/download links: `README.md`
- Current context: `PROJECT_CONTEXT.md`
- Detailed handoff: `AI_SESSION_HANDOFF.md`
- Master context: `Software_Project_Master_Context.md`
- Prompt archive: `PROMPT_ARCHIVE.md`
- Development log: `DEVELOPMENT_LOG.md`
- Backup manifest: `BACKUP_MANIFEST.md`
- Changelog: `CHANGELOG.md`
- Main application: `src/ResolverReach/`
- Installer and CI: `installer/ResolverReach.iss`, `.github/workflows/windows-build.yml`

## Repository history and backup limitations

Removing a file from the current tree does not erase older Git commits, historical documentation, or retained GitHub Actions records. Review Git history and available Actions artifacts before changing repository visibility. Backup branches preserve the source tree at the commit they reference; account-level ChatGPT memory and unrelated conversations cannot be exported through this repository integration.

## Design and upstream reference note

OnionFruit was inspected as an independent upstream UI/theme reference only. Its Tor client, networking components and runtime implementation are not Resolver Reach dependencies. Retain existing third-party notices and license records unless a separate license audit establishes that they are no longer required.
