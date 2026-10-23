# AI Session Handoff — Resolver Reach + Ollama GGUF Importer

Last verified: 2026-10-23 (America/New_York context: app repositories inspected and Resolver Reach v2.2.0 release read back from GitHub).

## Authoritative backup and operating rules

- Canonical private repository: https://github.com/PCUser2021/ResolverReach
- Default branch: `main`.
- Versioned full-tree backup branch for the latest Resolver Reach release: `backup/ResolverReach-2.2.0-2026-10-23` (intended to track the verified `main` tree).
- Read `PROJECT_CONTEXT.md`, `PROMPT_ARCHIVE.md`, `DEVELOPMENT_LOG.md`, `BACKUP_MANIFEST.md`, the current source, and workflow before making project changes.
- Archive repo-related user prompts/outcomes in private `PROMPT_ARCHIVE.md`. Do not put raw prompts, logs, or private project notes into the public Wand-Enhancer repository.
- Every shipped software change must bump product version according to SemVer, synchronize project/assembly/file/installer/workflow/release metadata, update `CHANGELOG.md`, build/test, read back release assets, and update persistent project notes. Documentation-only changes that do not change shipped behavior do not require a new product version.
- Never claim a source change, build, uploaded asset, successful test, or backup unless GitHub read-back verifies it.
- There is no global ChatGPT hook that guarantees every message in every conversation is automatically archived. Repository instructions work only when they are loaded and GitHub access is available.

## Current product: Resolver Reach 2.2.0

### Purpose and behavior
Windows 11 single-window DNS-over-HTTPS (DoH) resolver diagnostic application. It is not a VPN and does not change system DNS, routing, or general Internet traffic.

- Technologies: C# / .NET 10, Avalonia 11, FluentAvalonia, ReactiveUI, DynamicData.
- Project: `src/ResolverReach/ResolverReach.csproj`.
- Runtime identifiers: `win-x64;win-arm64`.
- App/project version: `2.2.0`; assembly/file version: `2.2.0.0`.
- The two UI paths test unique HTTPS Base URLs parsed from the current curl/curl DNS-over-HTTPS wiki or test pasted HTTPS endpoints. The app sends RFC 8484 wire-format DNS query for `example.com A`.
- Validation covers HTTP status, `application/dns-message` content type, transaction ID, response flags, question and DNS response code; HTTP 200/TLS alone is not considered success.
- Result statuses distinguish PASS (valid NOERROR), DNS ERROR (valid DNS exchange with non-NOERROR response), FAIL, and CANCELLED. A dead hostname, HTTP 403/404/502, wrong MIME type or invalid DNS packet is not a healthy resolver.
- Scan controls: maximum 512 unique endpoints per run; Escape cancels an active scan; bounded concurrency.
- Save Logs writes a user-selected ZIP archive containing `session-summary.txt`, `activity.log`, `resolver-results-history.csv`, `current-visible-results.csv`, and `session.json`. It captures current-process session history, not past application launches.
- Main-window header reserves 190 DIPs at right to avoid Windows caption controls overlapping the product/version badge.

### Resolver network resilience introduced in v2.1.1 and retained in v2.2.0
- 12-second connect timeout; 20-second HttpClient request timeout.
- Six concurrent probes rather than eight.
- One retry after a request timeout.
- One retry for transient HTTP statuses 408, 425, 429, 500, 502, 503, 504.
- Explicit HTTP/1.1 fallback after detected HTTP/2 protocol errors.
- Final scan summary separates transport failures from HTTP responses that fail DoH validation.

Important: no client can guarantee that all public third-party resolvers are online or standards-compliant. Resolver Reach reports observed behavior from the current machine/network/VPN. CI proves build/packaging only, not network reachability or UI interaction on the user's machine.

### Original icon overhaul in v2.2.0
- Visual direction: original premium dark titanium / glass with spectral cyan, blue, violet and magenta illumination; broad aesthetic inspiration from high-end music-production software without copying Apple Logic Pro artwork.
- Generated executable/installer icon: `src/ResolverReach/Assets/ResolverReach.ico`.
- SVG sources: `src/ResolverReach/Assets/Icons/*.svg`.
- XAML vector icon components in `src/ResolverReach/MainView.axaml`: product mark, catalog scanning, DoH check badge, paste links, Save Logs, empty state, and PASS/DNS ERROR/failure status indicators.
- Public app icon and UI preview: `public-distribution/assets/resolverreach-icon.svg`, `public-distribution/assets/resolverreach-preview.svg`. The preview is illustrative and not a capture from a running app.
- Accent color changed to spectral cyan in `src/ResolverReach/App.axaml`.

### Release v2.2.0 — verified
- Successful workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064
- Release: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0
- x64 installer (33,974,496 bytes): https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-x64.exe
- Native ARM64 installer (31,276,362 bytes): https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-arm64.exe
- Stable x64 URL: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Stable ARM64 URL: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe
- Both architecture jobs passed .NET restore, self-contained publish, Inno Setup compile, and artifact upload. Release job downloaded both installers and published v2.2.0.
- Workflow source: `.github/workflows/windows-build.yml`; installer: `installer/ResolverReach.iss`.
- CI validated packaging but did not run on physical ARM64 device, interactively test the UI, or perform a live resolver catalog scan.
- GitHub metadata last reported repository visibility `private`; public/anonymous release access requires a repository visibility change.

## Resolver diagnostics — known evidence and outstanding input

### Older log that was actually analyzed
File: `ResolverReach-Logs-20261023-032704.zip`, from Resolver Reach 2.1.0. The scan covered 266 endpoints and showed:
- 235 PASS
- 1 DNS ERROR (valid DNS response with RCODE REFUSED)
- 30 FAIL
- 236/266 (88.7%) valid DoH exchanges overall.

The 30 failures were reported as 11 timeouts around 5 seconds, 11 HTTPS/TLS/HTTP/2 errors, 4 HTTP status failures, 3 transaction-ID mismatches, and 1 HTML response rather than `application/dns-message`. v2.1.1 implemented longer timeouts, retries and HTTP/1.1 fallback.

### Newer log is still missing
The newer log discussed by the user is `ResolverReach-Logs-20261023-040016.zip`, previously linked via MediaFire. The direct download failed. The latest conversation attachment inventory and mounted files do not contain that ZIP; only the older 03:27 ZIP is mounted. Do not claim its contents were analyzed or infer exact v2.1.1 failure counts from the old log.

Next action required: ask the user to attach `ResolverReach-Logs-20261023-040016.zip` directly in chat. Once available:
1. Inspect all ZIP entries.
2. Calculate status distribution and cluster errors by detail, HTTP status, latency, hostname and transport.
3. Identify app-side bugs separately from dead/non-compliant third-party resolvers or machine/VPN/network behavior.
4. Implement only evidence-supported fixes; add regression tests when practical.
5. Bump product version for any shipped change; sync x64/ARM64 builds/installer/workflow/release metadata, changelog and persistent docs.
6. Build both architectures, verify the successful release and two actual assets, update backup branch and logs.
7. Ask for a fresh scan only if post-fix empirical comparison is needed.

## Ollama GGUF Importer (same repository)

- Current project is C#/.NET 10 Windows GUI under `src/OllamaGgufImporter`; workflow `.github/workflows/ollama-gguf-importer.yml`.
- It discovers/validates local `.gguf` files and imports through the Ollama-supported temporary Modelfile + `ollama create` flow. Never recommend copying GGUF bytes directly into Ollama's internal blob/manifest store.
- Handles multiple files/split GGUF shards, queues, activity logs, and user-visible version; preserve the GUI/open-and-click user preference.
- The repository-level `PROMPT_ARCHIVE.md` is the central private archive for repo-related prompts/outcomes from this conversation history.

## User workflow preferences
- Windows 11 Pro, generally prefers GUI / open-and-click instructions; avoid PowerShell unless needed.
- Wants explicit, direct downloads that have been verified against the actual release asset.
- Values thorough diagnosis and precise evidence. Never claim every DNS resolver can be made reachable.
- Wants every shipped software update versioned and project knowledge saved in GitHub.
- Keep repository records authoritative across future sessions; do not claim this file backs up all account-level ChatGPT memory.

## Backup completeness and limits
This file plus the full Git history/source/assets/releases and project logs preserve project-specific implementation context and work carried out in the repository. The current prompt archive stores the latest prompt and prior project work records. This does **not** export the entire account-level ChatGPT memory store, all unrelated chat conversations, or hidden/internal model state. Those cannot be written out by this repository integration. If full cross-session continuity is needed, use this handoff and archive as the persistent, versioned project memory.
