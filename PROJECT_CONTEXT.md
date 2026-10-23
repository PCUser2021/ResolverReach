# Project Context and Working Agreement

Last verified against repository source: 2026-10-22
Repository: https://github.com/PCUser2021/ResolverReach
Default branch: main (private repository)

> This file is the durable project overview for future maintainers and AI-assisted development sessions. Treat the current repository source and Git history as authoritative whenever this summary could be stale. Do not assume chat memory is complete.

## Repository contains two Windows desktop applications

### 1. Resolver Reach

- Product: Resolver Reach
- Current project version recorded in source: 1.0.0
- Target: Windows 11 25H2, x64 only; ARM64 is intentionally not targeted
- Stack: C# / .NET 10, Avalonia, FluentAvalonia, ReactiveUI, DynamicData
- Project: `src/ResolverReach/ResolverReach.csproj`
- Main source directory: `src/ResolverReach/`
- Icon: `src/ResolverReach/Assets/ResolverReach.ico`
- Installer: `installer/ResolverReach.iss`
- Build workflow: `.github/workflows/windows-build.yml`
- README: repository root `README.md`

Verified product behavior in README/source documentation:
- FluentAvalonia desktop window, system/light/dark themes, integrated endpoint results, and Paste Links support.
- Accepts HTTPS DoH endpoint URLs separated by lines or semicolons.
- Sends a real RFC 8484 wire-format DNS query for `example.com A`.
- Validates HTTP status, `application/dns-message` content type, transaction ID, response flags, question, and DNS response code. A TLS connection or HTTP success alone must not be reported as a valid DoH exchange.
- Reports endpoint status, HTTP status, latency, and DNS response details.
- Supports POST and falls back to GET for relevant POST rejection statuses (400, 405, 415, 501).
- “Check the Curl Resolvers” refreshes the current cURL DoH wiki catalog, extracts Base URL entries, deduplicates HTTPS endpoints, and tests with bounded concurrency and cancellable progress.
- Escape cancels a scan.
- “Save Logs” opens a native Save File dialog and exports the session summary and endpoint results to UTF-8 text.
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

- Both projects currently declare version 1.0.0 in their project configuration/build settings at the time of this audit.
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

Last audit note: source project files for both products declare version 1.0.0. The root README documents Save Logs in both products. This is a snapshot from 2026-10-22; re-check before representing it as current.
