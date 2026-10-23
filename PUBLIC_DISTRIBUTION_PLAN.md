# Public distribution publication plan — Resolver Reach

**Status checked on 2026-10-23:** the existing `PCUser2021/ResolverReach` repository remains private and contains source code for Resolver Reach and Ollama GGUF Importer. Do not change its visibility to public: GitHub explicitly states that making a private repository public makes the code visible to everyone and allows public forks.

The current connected GitHub actions available in this session support reading repository metadata and writing content/history files, but do not expose an action to create a new repository or change a repository's visibility. Therefore the release-only public repository has **not** been created or published by this session.

## Safe publication design

- Keep `PCUser2021/ResolverReach` private as the authoritative source repository.
- Create a separate public repository named `ResolverReach-Downloads` (or another non-conflicting name) containing only:
  - `README.md`
  - `LICENSE.txt`
  - `REPO_DESCRIPTION.txt`
  - `assets/resolverreach-icon.svg`
  - `assets/resolverreach-preview.svg`
  - a release asset `Resolver-Reach-2.0.0-Setup-x64.exe` (prefer a GitHub Release asset rather than source history).
- Do not copy the source tree, workflow logs that reveal source, debug symbols, or internal prompts into the public distribution repository.
- The screenshot asset here is expressly a vector design preview, not an actual captured app screenshot. Replace it with a genuine screenshot taken from the running app before labeling it as a screenshot.

## Requested description (repo summary)

Resolver Reach is a Windows 11 x64 DoH tester: check live cURL DNS-over-HTTPS resolver URLs or paste your own, validate real RFC 8484 DNS replies, and view status, latency, and results in one window. Personal/home use only; proprietary binary distribution.

## License language

See `LICENSE.txt`. The request's personal/home-use restriction and anti-reverse-engineering language are included. Enforceability of reverse-engineering prohibitions depends on applicable law and cannot be guaranteed solely by putting the wording in a README or license.

## Verification and backups

- Source repository remains private.
- Product version 2.0.0 source and project records live in the private Git history.
- Successful Windows x64 build/installer workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989
- Public distribution kit was created locally and should be provided as a downloadable ZIP; its embedded installer SHA-256 is recorded in `CHECKSUMS.txt`.
- ChatGPT's complete platform-level Memory store/conversation history is not exportable through the available tools. Repository-related prompts, decisions, implementation notes, build validation and handoff state are being preserved in `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, and `DEVELOPMENT_LOG.md`.
