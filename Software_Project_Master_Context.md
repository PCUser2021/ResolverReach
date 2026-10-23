# Software Project Master Context

Last verified: 2026-10-23

## Primary source of truth / durable project memory

- Main private GitHub repository: https://github.com/PCUser2021/ResolverReach
- Current full technical handoff: [AI_SESSION_HANDOFF.md](https://github.com/PCUser2021/ResolverReach/blob/main/AI_SESSION_HANDOFF.md)
- Project context: [PROJECT_CONTEXT.md](https://github.com/PCUser2021/ResolverReach/blob/main/PROJECT_CONTEXT.md)
- Prompt and outcome archive: [PROMPT_ARCHIVE.md](https://github.com/PCUser2021/ResolverReach/blob/main/PROMPT_ARCHIVE.md)
- Development log: [DEVELOPMENT_LOG.md](https://github.com/PCUser2021/ResolverReach/blob/main/DEVELOPMENT_LOG.md)
- Backup manifest: [BACKUP_MANIFEST.md](https://github.com/PCUser2021/ResolverReach/blob/main/BACKUP_MANIFEST.md)
- Changelog: [CHANGELOG.md](https://github.com/PCUser2021/ResolverReach/blob/main/CHANGELOG.md)
- Resolver Reach current source and workflows are authoritative; inspect them before claiming behavior or build status. Verify releases and artifacts through GitHub's Release API/workflow run.

## Project A — Resolver Reach 2.2.0

- Platform: Windows 11 x64 and native Windows ARM64.
- Technology: C# / .NET 10, Avalonia 11, FluentAvalonia, ReactiveUI, DynamicData.
- Project file: `src/ResolverReach/ResolverReach.csproj`; runtime IDs `win-x64;win-arm64`.
- Product version 2.2.0; assembly/file version 2.2.0.0.
- Purpose: single-window DNS-over-HTTPS resolver reachability and response-validity tester. It is not a VPN and does not modify Windows DNS/routing.
- Testing paths: fetch current HTTPS Base URLs from cURL's DNS-over-HTTPS wiki, or paste HTTPS DoH URLs one per line/semicolon-separated.
- Probe validates RFC 8484 DNS wire reply to `example.com A`: HTTP status, `application/dns-message` content type, DNS ID, response flag/opcode/question, and RCODE. HTTP 200/TLS connection alone is not considered success.
- Results: PASS = valid NOERROR; DNS ERROR = valid DoH exchange but DNS error code (for example REFUSED); FAIL = transport or invalid HTTP/DoH response; CANCELLED = cancelled probe.
- Limit: public endpoints can be offline, nonexistent, rate-limited, access-denied, misconfigured or non-compliant. No app can guarantee 100% of third-party resolvers are reachable. Separate actual application bugs from endpoint/network/VPN behavior.
- Resilience inherited from v2.1.1: 12s connect timeout; 20s HTTP timeout; six concurrent probes; one retry after client timeout; one retry for transient HTTP statuses 408/425/429/500/502/503/504; HTTP/1.1 fallback after detected HTTP/2 protocol errors.
- Save Logs ZIP contents: `session-summary.txt`, `activity.log`, `resolver-results-history.csv`, `current-visible-results.csv`, `session.json`. Captures current process history, not previous application sessions.
- Layout fix reserves 190 DIPs at the right of the extended header so version badge clears Windows caption controls.
- Icon overhaul: original dark titanium/glass with spectral cyan/blue/violet/magenta light; broad Logic Pro-inspired premium visual direction, not a copy. Main icon `src/ResolverReach/Assets/ResolverReach.ico`; SVG assets `src/ResolverReach/Assets/Icons/*.svg`; XAML vector controls `src/ResolverReach/MainView.axaml`; public mark/preview under `public-distribution/assets/`.
- Release v2.2.0: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0
- Verified workflow #39945785064: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064 (x64 + ARM64 publish, installers, and release job passed).
- x64 installer, 33,974,496 bytes: https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-x64.exe
- Native ARM64 installer, 31,276,362 bytes: https://github.com/PCUser2021/ResolverReach/releases/download/v2.2.0/ResolverReach-Setup-arm64.exe
- Stable installer URLs: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe and https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe
- Repository visibility last reported as private; public anonymous downloads need visibility change.

### Resolver log state

- Earlier log `ResolverReach-Logs-20261023-032704.zip` was inspected: 266 endpoints, 235 PASS, 1 DNS ERROR (RCODE REFUSED), 30 FAIL (88.7% valid exchanges). Failures: 11 ~5s timeouts, 11 HTTPS/TLS/HTTP2, 4 HTTP status errors, 3 mismatched DNS transaction IDs, 1 HTML instead of DNS MIME.
- The newer log `ResolverReach-Logs-20261023-040016.zip` was not present in the latest conversation attachment listing. The referenced MediaFire link failed to download. Do not claim that file has been analyzed.
- Next step for failure-specific fix: upload the newer ZIP directly to the chat; inspect each result, then code against evidenced causes, bump version for software behavior changes, run x64+ARM64 CI, publish and verify the new release, and synchronize the backup branch.

## Project B — Ollama GGUF Importer

- Current C#/.NET 10 Windows GUI source: `src/OllamaGgufImporter`; workflow: `.github/workflows/ollama-gguf-importer.yml`.
- Purpose: select/validate local GGUF files and register them with Ollama using a temporary Modelfile + supported `ollama create` flow.
- Never recommend copying a GGUF directly into Ollama's internal `.ollama/models` blob/manifest store.
- Preserves multi-file queues, split-GGUF shard handling, logs, visible product version and native icon.
- User previously reported Ollama at `C:\Users\googe\AppData\Local\Programs\Ollama`; conventional models store is `C:\Users\googe\.ollama\models` unless custom config changes it.

## Development / backup rules

- Append repo-specific prompts and outcomes to the private `PROMPT_ARCHIVE.md` before/after work; avoid committing secrets.
- For every shipped product change, increment semantic version, synchronize project metadata, assembly/file version, installer and workflow release tag, update changelog, test x64 and ARM64 packaging, verify uploaded files, and update project context/development log/backup manifest.
- Docs-only changes need not bump product version.
- After finalized docs, update `backup/ResolverReach-2.2.0-2026-10-23` to exactly match `main` and verify the branch SHAs.
- User prefers GUI/open-and-click Windows instructions; avoid PowerShell unless necessary.
- Be precise: say exactly which files/builds/tests were verified and what remains untested.

## Memory limitation

This file, the full GitHub source and Git history, the release assets, and the prompt/development archives preserve durable **project-specific** context. They are not a backup of all account-level ChatGPT memory, unrelated conversations, or hidden/internal model state. Do not claim that the complete ChatGPT memory database was exported or changed from a conversation.


## Latest diagnostic status (Prompt 015, 2026-10-23)

- User message named `ResolverReach-Logs-20261023-044828.zip`, but supplied only a codebase manifest: one binary file, zero chunks, visible ZIP signature prefix `PK\x03\x04\x14`.
- The actual ZIP is not in current conversation attachments, Library or `/mnt/data`. It has not been analyzed. Do not claim otherwise or treat old log statistics as new.
- Latest user handoff is archived in `PROMPT_ARCHIVE.md`; project context and development log record the blocker.
- Ask for a normal direct upload of the actual ZIP bytes before diagnosing/fixing 04:48 scan-specific failures.
