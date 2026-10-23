# Project Context and Working Agreement

Last verified against repository source: 2026-10-22
Repository: https://github.com/PCUser2021/ResolverReach
Default branch: main (visibility last verified private; user has requested public access)

> This file is the durable project overview for future maintainers and AI-assisted development sessions. Treat the current repository source and Git history as authoritative whenever this summary could be stale. Do not assume chat memory is complete.

## Repository contains two Windows desktop applications

### 1. Resolver Reach

- Product: Resolver Reach
- Current project version recorded in source after the 2026-10-23 Save Logs/titlebar/network reliability fixes: 2.1.1
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
- Version 2.1.1 has a visible Save Logs button in the results header. It opens a native ZIP save dialog and archives current-session activity, historical endpoint result rows, visible results, and session metadata.
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

Last verified build for product 2.1.0 succeeded in workflow run `39937042568`. Resolver Reach 2.1.1 reliability fixes are committed and the Windows build/release workflow is running; verify its outcome before claiming the new release. Repository metadata still reports `private`.


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


## Resolver Reach 2.1.1 — DoH probe resilience (2026-10-23)

- Uploaded ZIP analysis: 266 endpoints scanned; 235 PASS, 1 DNS ERROR (valid RCODE REFUSED), 30 FAIL. 236/266 (88.7%) returned valid DoH responses.
- Failure split: 11 timeouts at almost exactly 5,000 ms; 11 HTTPS/TLS/HTTP/2 failures; 4 HTTP status errors; 3 HTTP 200 replies with mismatched DNS transaction IDs; 1 HTML response instead of application/dns-message.
- Root weaknesses in v2.1.0: 5-second connect timeout, 10-second HttpClient timeout, and no HTTP/1.1 fallback after HTTP/2 resets.
- v2.1.1 raises connect timeout to 12s, request timeout to 20s, reduces concurrency from 8 to 6, retries once after client timeout or transient HTTP statuses (408/425/429/500/502/503/504), and retries HTTP/2 protocol resets over exact HTTP/1.1.
- Summary distinguishes transport failures (no HTTP status received) from HTTP responses that still failed DoH validation. A host-not-found, 403/404/502, unexpected content type, or wrong DNS transaction ID still correctly fails.
- Save Logs ZIP remains available and preserves current-session activity and all scan result history within one process.
- Product/assembly/file/installer/workflow version synchronized to 2.1.1 / 2.1.1.0 / tag v2.1.1. Wait for build/release verification before marking complete. A live post-fix scan is necessary to measure whether the 30 failures decrease.
