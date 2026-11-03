# Project Context and Working Agreement

Last verified: 2026-11-03
Repository: https://github.com/PCUser2021/ResolverReach
Default branch: main
Visibility verified: public (GitHub repository metadata checked 2026-11-03).

This file is the current project overview. Current source, Git history, GitHub Actions runs and published release assets are authoritative when summaries differ.

## Current application — Resolver Reach

**Current released Windows-only version:** Resolver Reach 2.5.0, published 2026-11-03. The release-preparation PR [#9](https://github.com/PCUser2021/ResolverReach/pull/9) was squash-merged to main as [`c37db14`](https://github.com/PCUser2021/ResolverReach/commit/c37db14bb14b592811cf4eb0447cc9587273dc27). The latest main-branch workflow [#41701054166](https://github.com/PCUser2021/ResolverReach/actions/runs/41701054166) passed protocol tests, Windows x64/native ARM64 builds, installer compilation, artifact verification, and release publication. Supported targets remain Windows x64 and native ARM64 only; the earlier macOS proposal remains withdrawn.

- Resolver Reach is the sole application maintained in the current source tree.
- Current main source and latest published GitHub Release: **v2.5.0**. Both architecture-specific installer assets are verified on the [v2.5.0 release page](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.5.0).
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

- Latest release: **v2.5.0**, published 2026-11-03: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.5.0
- Windows x64 installer: https://github.com/PCUser2021/ResolverReach/releases/download/v2.5.0/ResolverReach-Setup-x64.exe (52,323,107 bytes; SHA-256 `e73b725b7ca465a3cb1cadd5e3f551bc269017b7e69aaa7f008f7d5a5ec2ff3b`).
- Native Windows ARM64 installer: https://github.com/PCUser2021/ResolverReach/releases/download/v2.5.0/ResolverReach-Setup-arm64.exe (49,265,572 bytes; SHA-256 `622fcc20e17c7f4e2bf9260cb54c0df6e0b8f56736c3971b51e26cfeb19d1b44`).
- Main release workflow [#41701054166](https://github.com/PCUser2021/ResolverReach/actions/runs/41701054166) passed 17 deterministic tests, both Windows publish/installer jobs, artifact round-trip verification, and the release-publishing job.
- The release-preparation PR [#9](https://github.com/PCUser2021/ResolverReach/pull/9) passed its own complete test/build/installer/artifact matrix before merge.
- CI verifies compilation and packaging; it does not replace interactive GUI smoke testing or live network tests from Iran, Russia, China, or other target networks. A single client vantage point cannot certify global resolver availability or prove censorship.
- Product/operator country shown in the UI is not physical anycast-egress location; capability summaries are curated, and unknown DNSSEC/profile data remains unverified.

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


## Update — 2026-10-25: Resolver Reach 2.5.0 PR
- Created backup branch `backup/pre-resolver-reach-country-diagnostics-20261025` before changes.
- PR #7 (`feature/resolverreach-2.5.0-country-diagnostics`) proposes a Paste Links clear button, Fluent System Icon resources, a Windows 11 dark Fluent-inspired palette, provider country/flag profiles, curated capabilities, latency ordering of successful DNS responses, and detailed observed-error diagnostics.
- A timeout or reset is not classified as proof of censorship. Country values represent provider/operator origin, not guaranteed anycast location. Unknown endpoints remain unknown.
- The 2.5.0 proposal was merged and published on 2026-11-03; see the verified release and CI state above.
