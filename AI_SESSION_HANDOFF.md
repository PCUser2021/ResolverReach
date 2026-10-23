# AI Session Handoff — Resolver Reach

Last verified: 2026-10-23
Main branch contains Resolver Reach source version 2.3.0; latest published release verified in previous records is v2.2.0. Do not claim a v2.3.0 release until its release and assets are checked.

## Application and build

- Repository: https://github.com/PCUser2021/ResolverReach
- Main project: `src/ResolverReach/ResolverReach.csproj`
- Platform targets: Windows x64 and native ARM64 (`win-x64`, `win-arm64`).
- Stack: C# / .NET 10, Avalonia 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6, ReactiveUI.Reactive 26.0.1, DynamicData 9.5.0.
- UI source: `src/ResolverReach/MainView.axaml`, `MainWindow.axaml`, and their code-behind; icons under `src/ResolverReach/Assets/Icons/`.
- Installer: `installer/ResolverReach.iss`.
- Windows CI: `.github/workflows/windows-build.yml`.
- Product source version is 2.3.0; latest previously verified published release is v2.2.0. Verify current workflow and release metadata before reporting a newer release.

## Behavior that must be preserved

- One main window, two workflows: scan the live cURL DoH catalog or test user-supplied HTTPS endpoints.
- Send RFC 8484 DNS wire-format queries and validate HTTP status, the `application/dns-message` content type, transaction ID, response flags, echoed question and response code.
- Distinguish valid DNS error responses from transport failures and malformed/unreachable DoH responses.
- Bounded concurrency, cancellable scans, bounded transient retries, and HTTP/1.1 fallback following recognized HTTP/2 protocol errors.
- ZIP session export includes a summary, activity log, historical results, visible results and session/runtime metadata for the current process.
- Resolver Reach diagnoses endpoints; it must not change Windows DNS/proxy settings, act as a VPN, or route general traffic.

## Verified release and CI history

- Published v2.2.0: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0
- x64 installer: https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-x64.exe
- Native ARM64 installer: https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-arm64.exe
- Successful v2.2.0 workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064
- PR #2 merged as `e96baa4d822ce58617b8544e6167df94334ffea3`. Exact PR-head Windows x64/native ARM64 publish, installer compilation and artifact round-trip checks passed in run #39994207960. This did not publish v2.3.0.
- Current main may have additional commits after these recorded checks; read its latest commit and latest workflow run before claiming CI is green.

## Current diagnostic blocker

- The analyzed archive `ResolverReach-Logs-20261023-032704.zip` contained 266 endpoints: 235 PASS, 1 valid DNS ERROR (RCODE REFUSED), and 30 FAIL.
- The actual ZIP bytes for `ResolverReach-Logs-20261023-040016.zip` and `ResolverReach-Logs-20261023-044828.zip` were not present in the sessions that requested their analysis. Do not infer results from filenames or manifests.
- Next step: ask the user to attach the actual ZIP, inventory it, group endpoint failures by DNS/HTTP/transport details and latency, then implement only evidence-supported fixes and add regression tests where practical.

## User workflow preferences

- Windows 11 Pro 25H2 x64.
- Prefer GUI/open-and-click steps; avoid PowerShell unless necessary or explicitly requested.
- Prefer direct official download links and verify the actual release asset.
- Communicate exactly what was inspected, edited, built, tested and released. Do not treat CI packaging as a live network or interactive GUI test.
- Preserve current features unless the user explicitly asks to change or remove them.

## Repository continuity

- `README.md`: overview and download links.
- `PROJECT_CONTEXT.md`: current project summary and working agreement.
- `Software_Project_Master_Context.md`: compact technical handoff.
- `PROMPT_ARCHIVE.md`: append repository prompts and outcomes.
- `DEVELOPMENT_LOG.md`: append dated implementation and verification results.
- `BACKUP_MANIFEST.md`: record source snapshots and their limits.
- `CHANGELOG.md`: document repository and product changes.
- Keep secrets out of logs. Never claim a test, release, backup sync or memory export succeeded unless verified.

## Cleanup and backup state

- The current main tree is intended to contain Resolver Reach and its installer/build pipeline only.
- A snapshot branch, `backup/pre-remove-ollama-gguf-importer-2026-10-23`, preserves the pre-cleanup source tree at its recorded commit.
- Deleting tracked files from current main does not rewrite older commits or automatically erase historical workflow logs/artifacts. This operation did not rewrite Git history.
- Full account-level ChatGPT memory and unrelated chats cannot be exported by the GitHub connector.

## Upstream reference note

OnionFruit was inspected as a separate C#/.NET Tor desktop application and a UI/theme reference only. No OnionFruit Tor engine or runtime dependencies should be added to Resolver Reach merely to imitate its technology stack.
