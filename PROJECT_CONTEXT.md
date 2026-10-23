# Project Context and Working Agreement

Last verified against repository source: 2026-10-22
Repository: https://github.com/PCUser2021/ResolverReach
Default branch: main (visibility last verified private; user has requested public access)

> This file is the durable project overview for future maintainers and AI-assisted development sessions. Treat the current repository source and Git history as authoritative whenever this summary could be stale. Do not assume chat memory is complete.

## Repository contains two Windows desktop applications

### 1. Resolver Reach

- Product: Resolver Reach
- Current project version recorded in source after the 2026-10-23 Save Logs/titlebar fix: 2.1.0
- Target: Windows 11 25H2, x64 only; ARM64 is intentionally not targeted
- Stack: C# / .NET 10, Avalonia, FluentAvalonia, ReactiveUI, DynamicData
- Project: `src/ResolverReach/ResolverReach.csproj`
- Main source directory: `src/ResolverReach/`
- Icon: `src/ResolverReach/Assets/ResolverReach.ico`
- Installer: `installer/ResolverReach.iss`
- Build workflow: `.github/workflows/windows-build.yml`
- README: repository root `README.md`

Verified product behavior in README/source documentation:
- One maximized FluentAvalonia window with a dark slate palette, a prominent red status strip, rounded slate cards, and a restrained pink accent. The interface exposes exactly two testing paths: live cURL catalog testing and paste-links testing.
- Accepts HTTPS DoH endpoint URLs separated by lines or semicolons.
- Sends a real RFC 8484 wire-format DNS query for `example.com A`.
- Validates HTTP status, `application/dns-message` content type, transaction ID, response flags, question, and DNS response code. A TLS connection or HTTP success alone must not be reported as a valid DoH exchange.
- Reports endpoint status, HTTP status, latency, and DNS response details.
- Supports POST and falls back to GET for relevant POST rejection statuses (400, 405, 415, 501).
- “Check the Curl Resolvers” refreshes the current cURL DoH wiki catalog, extracts Base URL entries, deduplicates HTTPS endpoints, and tests with bounded concurrency and cancellable progress.
- Escape cancels a scan.
- Version 2.1.0 has a visible Save Logs button in the results header. It opens a native ZIP save dialog and archives current-session activity, historical endpoint result rows, visible results, and session metadata.
- It is a diagnostic utility, not a VPN or Windows system DNS manager. It must not change Windows DNS settings or route general traffic.

### 2. Ollama GGUF Importer

- Product: Ollama GGUF Importer
- Current project version recorded in source/build workflow: 1.0.0
- Target: portable Windows x64 application
- Stack: C# / .NET 10 Windows Forms
- Project: `src/OllamaGgufImporter/OllamaGgufImporter.csproj`
- Main source directory: `src/OllamaGgufImporter/`
- Icon: `src/OllamaGgufImporter/Assets/OllamaGgufImporter.ico`
- Build workflow: `.github/workflows/ollama-gguf-importer.yml`
- README: `src/OllamaGgufImporter/README.md`

Verified product behavior in README/source documentation:
- Graphical workflow for locating/selecting local `.gguf` model files and importing them into a running local Ollama service.
- Validates the GGUF magic header before enabling import.
- Supports selecting multiple files and common split shard sets such as `model-00001-of-00003.gguf`; keep all shards together and do not rename them.
- Derives safe model names from filenames; model names are lowercase/sanitized and collisions receive numeric suffixes.
- Registers a model through Ollama's supported temporary Modelfile + `ollama create` process. Never tell the user to copy GGUF files directly into Ollama's internal blob/manifest store as an import method.
- Reads source files in place; the app does not upload or move them. Ollama ingests the model into its own configured model store.
- Detects the current user's conventional Ollama store location (`%USERPROFILE%\\.ollama\\models`) while acknowledging that custom `OLLAMA_MODELS` configuration or service settings can change it.
- Import requires Ollama's CLI and a running/local service. Model architecture and GGUF metadata must be supported by installed Ollama; this application does not convert or quantize weights.
- “Save Logs” opens a Save As dialog and exports the activity log, current application status, and current import-queue states to a UTF-8 text file.
- Product version is displayed in the main application header; executable and window use the dedicated app icon.
- The workflow builds a self-contained, single-file Windows x64 app and ZIP artifact named `Ollama-GGUF-Importer-Windows-x64`, with `README.txt`. Workflow artifact retention is configured for 14 days.

## User's working preferences

- User is on Windows 11 Pro 25H2 x64.
- Prefer direct GUI/open-and-click workflows, especially for installing, importing, and troubleshooting Windows software.
- Do not default to PowerShell or CLI instructions when a GUI method is feasible. Use commands only when requested or technically necessary, and explain why when necessary.
- User values direct, official links, working builds, current version verification, high technical precision, and clear acknowledgement of what has and has not been tested.
- Do not make the user repeat project information already present in this repository.

## Mandatory project continuity / logging policy

For every substantive task related to either application:
1. Read this file and the relevant current source/README/workflow before proposing changes.
2. Inspect current Git status/history or the latest branch state when available; never rely on stale chat summaries alone.
3. Preserve existing features unless the user explicitly asks to remove or replace them. In particular, keep product-version information and Save Logs functionality in both applications.
4. After making a code, UI, asset, build, configuration, or documentation change, append a dated entry to `DEVELOPMENT_LOG.md` in the same task. Include affected files, what changed and why, tests/build commands actually run and their actual outcomes, commit/PR/build artifact references, and any remaining limitations. Never invent tests, success statuses, versions, or artifact availability.
5. Log important project-related decisions, requirements, bug reports, failed approaches, environment-specific findings, and deferred work when they materially affect future work. Keep entries chronological and append-only; correct mistakes with a follow-up entry instead of silently rewriting history.
6. Before finishing a project task, verify the final source/state and ensure the log describes what was actually completed.
7. If a future session cannot write to the repository, state that plainly and provide the exact log entry for the user to commit. Do not claim an item was logged until the repository change is confirmed.
8. Use the latest GitHub source and history as the source of truth for code. This context file and the development log are summaries/audit records, not substitutes for source code, commit diffs, issues, or workflow logs.

## Release/version checks

- Resolver Reach is being advanced to 2.0.0 for its full UI replacement. Ollama GGUF Importer remains 1.0.0 unless changed in a separate shipped update.
- Before claiming “latest version,” verify the current project file, most recent successful GitHub Actions run, and the availability of its artifact. A workflow page existing is not proof a successful artifact exists.
- The GGUF Importer workflow sets a 14-day artifact retention period, so an old build may expire.
- Resolver Reach workflow: `.github/workflows/windows-build.yml`.
- GGUF Importer workflow: `.github/workflows/ollama-gguf-importer.yml`.

## Repository map

- Root overview: `README.md`
- Persistent context: `PROJECT_CONTEXT.md`
- Append-only log: `DEVELOPMENT_LOG.md`
- Resolver Reach source: `src/ResolverReach/`
- GGUF Importer source: `src/OllamaGgufImporter/`
- Resolver Reach installer: `installer/ResolverReach.iss`
- Resolver Reach CI: `.github/workflows/windows-build.yml`
- GGUF Importer CI: `.github/workflows/ollama-gguf-importer.yml`

Last verified 2026-10-23: Resolver Reach 2.1.0 build and Release succeeded in workflow run `39937042568`; release asset is uploaded. Repository metadata still reports `private`. See `PUBLIC_DISTRIBUTION_PLAN.md`, `BACKUP_MANIFEST.md`, and the latest development-log entry.


## Cross-repository prompt backup and restore policy

- Central private prompt archive: [PROMPT_ARCHIVE.md](PROMPT_ARCHIVE.md). It preserves repository-related user prompts and task outcomes across all repositories visible to the connected GitHub account.
- At the start of every repository-related task, read the latest relevant archive entries and restore context from the target repository's current `AGENTS.md`, project context, README, source, recent commits, and CI runs.
- Before substantive work, append the user's repository-related prompt exactly to `PROMPT_ARCHIVE.md` and commit it. Do not claim capture until the commit and file are verified.
- After work, append a separate outcome to `PROMPT_ARCHIVE.md` and an implementation/test entry to `DEVELOPMENT_LOG.md`. Record only checks actually performed.
- Current accessible repository inventory last checked 2026-10-22: `PCUser2021/ResolverReach` (private, central archive) and `PCUser2021/Wand-Enhancer` (public).
- Never place verbatim user prompts in the public `Wand-Enhancer` repository. Keep prompt text in the private central archive; public repositories may have generic instructions and non-sensitive work summaries only.
- Never save credentials or secret values in logs; redact them. Scope is every repository-related user prompt, not unrelated general-chat messages.
- Restore means reconstructing project requirements and unfinished task state from the archive and current source. Do not automatically roll back or overwrite repository code without explicit approval.
- Hard limitation: this policy does not create a ChatGPT platform event hook. It cannot guarantee that every message from every conversation is intercepted if a session does not load these instructions or cannot write to GitHub. If GitHub writing is unavailable, say so and provide a ready-to-commit archive entry.


## Mandatory version bump and knowledge-persistence policy

### Application versioning
- Whenever an update to a user-owned software product changes shipped source code, user-visible behavior/UI, dependencies, assets, configuration, packaging, or executable output, create a **new product version**. Never ship two different software updates under the same product version.
- Use semantic versioning where applicable: patch for fixes/refinements, minor for backward-compatible features, major for breaking changes. Use the project's existing versioning convention if it is different, but still advance the version on each shipped software update.
- Keep the version consistent everywhere it is exposed or consumed: project/package metadata, assembly/file versions, in-app About/header/version display, installer metadata, workflow publish arguments, release/tag names, and user-facing documentation. Search for duplicated literals so an old version is not left behind.
- Update the relevant user-facing `CHANGELOG.md` (or project changelog) with the version, date, changes, compatibility notes, and known limitations. If no changelog exists, create one. Do not fabricate dates, tested behavior, or test results.
- Verify the built executable/package reports the intended version; run the applicable build/tests and inspect the resulting artifact. Do not say a version was released until the artifact/release is confirmed.
- Documentation-only or internal archive/policy edits that do not change the shipped software do **not** require incrementing the application's binary version. Third-party dependencies keep their upstream version numbers unless dependency upgrades are explicitly part of the task.

### Persist project knowledge after every repository-related prompt
- Append each repository-related prompt to the central private `PROMPT_ARCHIVE.md` before substantive work. Keep secrets redacted.
- Persist new durable requirements, design decisions, discoveries, environment findings, bug causes, failed approaches, unfinished tasks, and user preferences in the appropriate repository context/instructions file. Do not leave essential knowledge only in chat.
- After task completion, append an outcome to the private prompt archive and relevant development log. For public repositories, put only non-sensitive summaries in the public-safe AI work log; raw prompt text remains in the private central archive.
- Include file paths, commit references, validation performed and results, artifact/release links, and outstanding issues. State when a backup/log could not be written; never claim persistence unless the commit has been verified.
- At the beginning of a later task, read the private archive and current repository instructions/source to restore relevant context. Treat source, history, CI logs, and artifacts as authoritative over stale summaries.
- This policy is a required procedure for sessions that load these instructions and have GitHub access. It does not create an automatic ChatGPT-wide message interception hook and must not be represented as doing so.


## 2026-10-23 — Resolver Reach 2.0.0 single-window redesign

- The user's explicit scope is exactly two visible testing paths: **Test cURL Resolvers** and **Paste links → Test Resolvers**. Inputs, status, progress, and endpoint results stay on the one main window.
- The supplied OnionFruitt screenshot is a visual reference only: use dark slate panels, a prominent red status strip, rounded cards/rows, and restrained pink accents. Do not add its sidebar/navigation, because the user explicitly disallowed extra options or windows.
- In v2.0.0 the visible theme picker and Paste Links menu were removed. Version 2.1.0 adds a Save Logs button and ZIP file-picker flow back to the main UI; Escape-to-cancel remains.
- Version 2.0.0 was selected because the GUI interaction model is replaced. Keep project/package, assembly/file, HTTP User-Agent, installer, workflow, and documentation version metadata synchronized on subsequent shipped updates.
- Preserve RFC 8484 testing and DNS-response validation, live cURL wiki extraction, de-duplication, bounded concurrency, the 512-URL limit, and in-window results/progress. Results are point-in-time and cannot guarantee universal reachability.



## MIT licensing, Release downloads, and public visibility — 2026-10-23

- The user's latest instruction replaces the earlier personal/home-use-only restriction with permission for anyone to use the original project source for any purpose. Use the MIT License in root `LICENSE`, compatibility `LICENSE.txt`, and `public-distribution/LICENSE.txt`.
- MIT allows use, copying, modification, publishing, distribution, sublicensing and sale, subject to preservation of the copyright/license notice. It applies only to project-owned original code/docs/assets; third-party dependencies and trademarks retain their respective licenses.
- Root README provides `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe` as the stable direct download path. CI is being configured to produce a `v2.0.0` GitHub Release and attach `ResolverReach-Setup-x64.exe` after successful build on a designated release commit. Verify the actual release and asset before calling the link active.
- GitHub repository metadata last reported `private`. License permissions do not change repository visibility; public code/download access still requires the owner to set the repository to Public in GitHub Settings. This session's GitHub connector does not offer a visibility-change operation.
- Public front page includes the app icon and a clearly labelled layout preview. The preview is an illustration, not a screenshot captured from the live app.
- Git history contains both Resolver Reach and Ollama GGUF Importer sources. If the source repository is made public, all tracked files/history and Actions logs will also be visible.
- Full platform-level ChatGPT Memory and every historical chat cannot be exported through the available tools. Project prompts, decisions, state and test outcomes visible in this work are recorded in `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, `DEVELOPMENT_LOG.md`, and `BACKUP_MANIFEST.md`.


## MIT licensing, Release downloads, and public visibility — 2026-10-23

- The user's latest instruction replaces the earlier personal/home-use-only restriction with permission for anyone to use the original project source for any purpose. Use the MIT License in root `LICENSE`, compatibility `LICENSE.txt`, and `public-distribution/LICENSE.txt`.
- MIT allows use, copying, modification, publishing, distribution, sublicensing, and sale, subject to preservation of the copyright/license notice. It applies to project-owned original code/docs/assets; third-party dependencies and trademarks retain their respective licenses.
- Root README provides `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe` as the stable direct download path. CI is being configured to produce a `v2.0.0` GitHub Release and attach `ResolverReach-Setup-x64.exe` after successful build on a designated release commit. Verify the actual release and asset before calling the link active.
- GitHub repository metadata last reported `private`. License permissions do not change repository visibility; public code/download access still requires the owner to set the repository to Public in GitHub Settings. This session's GitHub connector does not offer a visibility-change operation.
- Public front page includes the app icon and a clearly labelled layout preview. The preview is an illustration, not a screenshot captured from the live app.
- Git history contains both Resolver Reach and Ollama GGUF Importer sources. If the source repository is made public, all tracked files/history and Actions logs will also be visible.
- Full platform-level ChatGPT Memory and every historical chat cannot be exported through the available tools. Project prompts, decisions, state, and test outcomes visible in this work are recorded in `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, `DEVELOPMENT_LOG.md`, and `BACKUP_MANIFEST.md`.


## Verified Release asset — 2026-10-23
- Published Release: [Resolver Reach 2.0.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.0.0).
- Workflow #39933931024 completed successfully, including Windows x64 publish, installer compilation, and release asset upload.
- Stable URL: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- Repo visibility last read as private; MIT licensing is committed but public access still requires the owner to switch the repository to Public.
- Future executable updates require a new application version and corresponding tag; ordinary docs/license updates do not increment the product version.


## Resolver Reach 2.1.0 — titlebar layout fix and ZIP log export (2026-10-23)

- Fixed the top-right title/version collision shown in the user's screenshot. The header reserves a 190-DIP right safe area so the product/version badge stays clear of Windows minimize/maximize/close controls while keeping the extended titlebar design.
- Added an explicit `Save Logs` button in the Resolver results header. The button opens Avalonia's native `StorageProvider.SaveFilePickerAsync` with `*.zip` as the file type and a timestamped suggested filename.
- ZIP archive entries: `session-summary.txt`, `activity.log`, `resolver-results-history.csv`, `current-visible-results.csv`, and `session.json`.
- The in-memory log captures application session/runtime info; each cURL or pasted-URL scan; every endpoint outcome with HTTP status, latency, DNS summary/detail; cancellation/failure and final scan summary. Historic results are preserved across scans during one app session even though the on-screen result list still shows the current scan.
- Product version is 2.1.0 (assembly/file version 2.1.0.0). Release workflow and installer metadata are synchronized to `v2.1.0`; successful release verified in workflow #39937042568.
- Export uses a file picker and writes a ZIP at the selected destination. Canceling the picker does not create a file. The ZIP exports in-memory session data only; it does not claim to include sessions from prior app launches.


## Resolver Reach 2.1.0 verification — 2026-10-23

- Fixed titlebar collision via a 190-DIP right-side header safe area.
- Save Logs creates a user-selected ZIP containing session summary, activity log, historical endpoint results across all runs in this app process, current visible results, and JSON session/runtime metadata.
- Product/assembly/file/installer versions: 2.1.0 / 2.1.0.0 / 2.1.0.0 / 2.1.0.
- Successful CI and release run: #39937042568. Published release v2.1.0 asset `ResolverReach-Setup-x64.exe` (33,980,655 bytes).
- No interactive desktop UI test or end-to-end click-to-ZIP test was run; the Windows CI build and package/release upload succeeded.
- Repository visibility is still private at the final metadata check.


## 2026-10-23 — JDownloader Reach prototype

- New application: **JDownloader Reach 0.1.0 prototype**.
- Project: `src/JDownloaderReach/JDownloaderReach.csproj`; stack: C# / .NET 10, Avalonia, FluentAvalonia.
- Isolated development branch: `feature/jdownloader-reach-prototype`. Do not rewrite `main` or remove existing projects when continuing this integration.
- Prototype functions: launch a user-selected JDownloader executable; call the legacy local API at fixed loopback URL `http://127.0.0.1:3128/` to check engine state, add links to LinkGrabber, query a package snapshot, and request download start; launch Resolver Reach and Ollama GGUF Importer; persist executable paths; export activity/settings/runtime details to ZIP.
- Workflow `.github/workflows/jdownloader-reach.yml` publishes the shell and both existing apps into one Windows x64 ZIP artifact. It does not package a JDownloader Java distribution.
- License boundary: no JDownloader Java implementation is copied into this prototype. The source mirror was inspected for API/build/license reference. JDownloader source headers specify GPL-3.0-or-later; before distributing a future engine-source bundle or derivative, perform a full dependency/license audit and comply with source-distribution obligations. Resolver Reach source is MIT; its third-party notices remain applicable.
- Security: API address is hard-coded to `127.0.0.1`; no remote host or credentials are supported. The deprecated local API may be unauthenticated. Do not expose port 3128 outside loopback.
- Limits: no full Java-to-C# source conversion; the engine, extractors, plugin ecosystem, premium-account logic, update system, and original UI remain in JDownloader. This is a functional integration prototype, not a finished replacement.
- Validation policy: do not claim compile success or live API testing until a corresponding Windows Actions run or real installation test is observed. No local build or interactive API test was performed during authoring.
- Preserve existing behavior and versions of Resolver Reach 2.1.1 and Ollama GGUF Importer 1.0.0. The new prototype is versioned independently as 0.1.0.


## 2026-10-23 — JDownloader Reach 0.2.0 x64 and ARM64 installer packaging

This section supersedes the earlier statement that the prototype was x64-only and ZIP-only.

- **Version:** JDownloader Reach is now 0.2.0 (assembly/file version 0.2.0.0). Resolver Reach remains 2.1.1; Ollama GGUF Importer remains 1.0.0.
- **Installers:** `installer/JDownloaderReach-x64.iss` builds `JDownloaderReach-Setup-x64.exe`, restricted to x64 Windows. `installer/JDownloaderReach-arm64.iss` builds `JDownloaderReach-Setup-arm64.exe`, restricted to Windows ARM64.
- **Native application payload:** The workflow publishes the shell and both repository apps for `win-x64` or `win-arm64`. `src/OllamaGgufImporter/OllamaGgufImporter.csproj` now selects `PlatformTarget=x64` for x64 and `ARM64` for the ARM64 runtime. The two Avalonia projects declare both runtime identifiers.
- **Verification:** A PowerShell PE-header check validates all three EXEs have machine type `0x8664` (x64) or `0xAA64` (ARM64) as selected by the matrix, before the installer is compiled.
- **Distribution contents:** Each installer includes JDownloader Reach, native Resolver Reach, native Ollama GGUF Importer, shell setup instructions, the root MIT license, Resolver Reach third-party notices, and the referenced `THIRD-PARTY-LICENSES/ONIONFRUIT-LGPL-3.0.md` file.
- **Workflow:** `.github/workflows/jdownloader-reach.yml` creates separate GitHub Actions artifacts named `JDownloaderReach-Setup-x64` and `JDownloaderReach-Setup-arm64`, with 30-day retention. This is artifact distribution, not a permanent GitHub Release.
- **Security/product boundary:** JDownloader's Java engine is still not bundled. The shell operates an existing JDownloader instance via loopback-only legacy API controls and launches the two .NET tools.
- **Current CI note:** The build before the license-copy correction succeeded for both architectures in workflow run `39943167275`. The rebuild including the third-party license file is in progress under workflow run `39943711695`; update the outcome after inspecting that exact run.


### Final verification — x64 and ARM64 installers including third-party license text (2026-10-23)

- Final successful workflow: [#39943711695](https://github.com/PCUser2021/ResolverReach/actions/runs/39943711695), commit `8a2feed6059ee3fc8d6590f5b679e26a79a8b711`; both matrix jobs completed with `success`.
- Artifacts: `JDownloaderReach-Setup-x64` (artifact ID `12496179768`, 97,832,112 bytes) and `JDownloaderReach-Setup-arm64` (artifact ID `12497252555`, 89,365,276 bytes). Both artifacts include the native three-app bundle, readme, MIT license, Resolver Reach third-party notices, and the actual `THIRD-PARTY-LICENSES/ONIONFRUIT-LGPL-3.0.md` text.
- Downloaded Setup EXEs, extracted from those artifacts and checked non-empty: x64 SHA-256 `5b865279309ccc790f80be05fa92cd3e177c0a7736f416b431d27fa7e442bc8f`; ARM64 SHA-256 `d2d4a1a6309038fbd7ba76324d6132c1b455ce9da7c3ffe25334e0ef2ced5f48`.
- The CI PE-machine validation step, Inno Setup compilation, and artifact upload succeeded for both architectures.
- No interactive install test on a physical ARM64 device was performed; successful cross-target compilation and packaging do not establish all runtime integrations against an installed JDownloader instance.


## Final installer verification — 2026-10-23

JDownloader Reach 0.2.0 has two successful architecture-specific installers from workflow run 39943711695. The x64 installer artifact is 12496179768 and the ARM64 artifact is 12497252555. Both include native builds of the shell, Resolver Reach, and Ollama GGUF Importer; the license copy step includes the third-party LGPL text referenced by Resolver Reach notices. PE machine-type checks, Inno Setup compilation, and artifact upload passed in both jobs.

Local extracted installers:
- x64 SHA-256: 5b865279309ccc790f80be05fa92cd3e177c0a7736f416b431d27fa7e442bc8f
- ARM64 SHA-256: d2d4a1a6309038fbd7ba76324d6132c1b455ce9da7c3ffe25334e0ef2ced5f48

No interactive install test on physical ARM64 or x64 hardware was performed. JDownloader's Java engine remains a separate prerequisite.


## Current branch state — JDownloader Reach 0.3.0 (2026-10-23)

- **Project page:** https://github.com/PCUser2021/ResolverReach/tree/feature/jdownloader-reach-prototype
- **Active branch:** `feature/jdownloader-reach-prototype`. The default branch `main` remains separate; do not merge the prototype into `main` without the user's explicit instruction.
- **Current version:** JDownloader Reach 0.3.0 (assembly/file version 0.3.0.0), targeting `win-x64` and `win-arm64`.
- **Current contents:** a .NET 10/Avalonia shell, an existing separately installed JDownloader engine integration through the optional loopback-only legacy API, and a launcher for Resolver Reach. It supports launching JDownloader, API state check, LinkGrabber submission, package snapshot, start-downloads request, executable-path persistence, and Save Logs ZIP export.
- **Explicitly removed in 0.3.0:** the Ollama GGUF Importer integration from the JDownloader Reach app (UI card, path detection, saved setting, browse/launch handlers), installer shortcuts/payload, and JDownloader Reach build workflow. The standalone `src/OllamaGgufImporter/` project and `.github/workflows/ollama-gguf-importer.yml` remain unchanged because they are separate repository products, not a JDownloader Reach dependency.
- **Current packaging:** each architecture-specific installer contains JDownloader Reach and Resolver Reach only, with required MIT and third-party license texts and setup instructions. JDownloader's Java engine is not bundled.
- **Versioned backups:** `backup/JDownloaderReach-full-2026-10-23` preserves the full pre-removal repository state; `backup/JDownloaderReach-0.3.0-clean-2026-10-23-r2` snapshots the cleaned JDownloader Reach 0.3.0 state with the workflow syntax correction at commit `b86b6743c1d8da8e597bd9e3cbf10dcecfe35305`. Previous 0.2.0 installer artifacts are associated with workflow run https://github.com/PCUser2021/ResolverReach/actions/runs/39943711695; their hashes are recorded in the development log and backup branch.
- **Verification caveat:** the 0.2.0 x64 and ARM64 installers had successful build/PE validation. The revised 0.3.0 source must be rebuilt in GitHub Actions; no physical-device GUI/API integration test has been run.
- **Privacy/publication:** the current repository metadata is private. It is also a central repository containing other projects and a private prompt archive. Do not expose this entire repository merely to publish JDownloader Reach. A dedicated public repository would be the correct publication boundary, but the current GitHub connector has no repository-creation or visibility-mutation operation. Never claim the project is public until verified.
- **Project-specific memory:** see `JDOWNLOADER_REACH_MEMORY.md`. Raw prompts stay in this private repository's `PROMPT_ARCHIVE.md`; the memory file is scoped to JDownloader Reach and excludes unrelated personal ChatGPT memories.
