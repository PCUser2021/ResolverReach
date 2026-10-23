# Private Cross-Repository Prompt Archive and Recovery Ledger

**Private home:** https://github.com/PCUser2021/ResolverReach  
**Why this lives here:** This repository is private. Do not copy verbatim user prompts or private conversation details into public repositories.

## Purpose and scope

This is the central, append-only record for user prompts that concern any of the user's GitHub repositories, plus the task outcomes needed to resume work across conversations. It is intended to restore project context after a chat is lost or ChatGPT Memory is unavailable.

Repository inventory last checked on 2026-10-22 through the connected GitHub account:
- `PCUser2021/ResolverReach` — private; default branch `main`. Holds this central archive, `PROJECT_CONTEXT.md`, and `DEVELOPMENT_LOG.md`.
- `PCUser2021/Wand-Enhancer` — public; default branch `master`. Its repository instructions are in `AGENTS.md`. Never put raw prompt text into that public repository.

The connected account showed these two repositories at the time of the check. Re-enumerate accessible repositories when doing cross-repository work; do not assume this list can never change.

## Required procedure for every repository-related user prompt

1. **Capture first:** Before substantive work, append the user's repository-related prompt verbatim below as a new numbered entry. Include the date, target repository/repositories, and status. For very long prompts, archive the full text or a faithful exact source file; do not replace it with a summary.
2. **Protect secrets:** Never persist passwords, API keys, access tokens, recovery codes, private keys, or other credentials. Replace secret values with `[REDACTED SECRET]`. Do not copy unrelated sensitive personal information into this code-project archive.
3. **Restore context:** Read the most recent relevant entries here; read the target repository's `AGENTS.md`, `PROJECT_CONTEXT.md` and/or README; inspect current source and recent commit/workflow state. Reconstruct unfinished requirements from these records before asking the user to repeat information.
4. **Work from source:** The current repository state, diffs, tests, issues, and CI logs are authoritative. Prompt text records intent; it does not establish that an implementation worked.
5. **Record outcome:** After responding or making changes, append a separate outcome entry stating what was done, exact files/commits touched, verification actually performed, links to builds/artifacts where available, and unresolved items. Do not silently rewrite old records.
6. **Commit the backup:** The archive update itself must be committed to this private repository and verified. If writing access is unavailable or an update fails, disclose that immediately and provide a ready-to-commit entry. Do not claim a prompt was backed up unless the commit is confirmed.
7. **Public repositories:** Raw prompts stay in this private central archive. Public repositories may contain a generic workflow policy and non-sensitive work summaries only.
8. **Scope:** Capture every prompt related to any of the user's repositories, including questions, bug reports, feature requests, corrections, release/download requests, and instructions. Unrelated general-chat prompts are outside this repo archive.
9. **Restore meaning:** On a new project session, use this archive and repository instructions to restore context and unfinished-task state. Do not automatically reset/overwrite source code to an older commit; destructive code rollback requires explicit user approval.

## Operational limitation — do not misrepresent automation

ChatGPT does not expose a guaranteed event hook here that automatically writes every message to GitHub before the assistant responds, and this file does not itself create such a platform integration. This is a strict operating procedure for any assistant session that can read and write the connected GitHub repository. If a session does not load this instruction or lacks GitHub access, it cannot guarantee capture. Never promise that the process is globally automatic across every ChatGPT conversation.

## Prompt archive

The following seed entries were reconstructed from repository-related prompts visible in the current conversation on 2026-10-22 (America/Los_Angeles). They are not claimed to be a complete export of older conversations.

### Prompt 001 — version and Save Logs check
- Date: 2026-10-22
- Target: `PCUser2021/ResolverReach` — Ollama GGUF Importer
- Exact prompt:

> now give me the latest versiom of Ollama GGUF Importer  
> did you remember that you've add version info save logs

### Prompt 002 — project memory continuity
- Date: 2026-10-22
- Target: `PCUser2021/ResolverReach`, `PCUser2021/Wand-Enhancer`
- Exact prompt:

> how to save your memory 100% about Resolver Reach Ollama GGUF Importer

### Prompt 003 — request for persistent repository context and logging
- Date: 2026-10-22
- Target: all repositories accessible to the connected GitHub account
- Exact prompt:

> it doesn't work that way  
> just do something to always remember my repos and always log everything to the project

### Prompt 004 — latest binaries
- Date: 2026-10-22
- Target: `PCUser2021/ResolverReach`
- Exact prompt:

> now give me the latest version of Resolver Reach — Windows 11 25H2 x64 and Ollama GGUF Importer — portable Windows x64

### Prompt 005 — current request to automate prompt backup and restore
- Date: 2026-10-22
- Target: all repositories accessible to the connected GitHub account
- Exact prompt:

> always auto backup every single prompt for all of my repos and auto restore it

### Outcome 005 — repository-side backup and recovery policy setup
- Status: implementation in this commit set; verify commits and resulting files before claiming complete.
- Planned: centralize verbatim repository-related prompt entries in this private file; direct AI sessions to restore context from the archive and current repository state; keep raw prompts out of the public Wand-Enhancer repository; document the policy in both repositories.
- Verification must be recorded only after the GitHub writes succeed.


### Outcome 005 — Completed repository-side setup
- Date: 2026-10-22 (America/Los_Angeles)
- **Private central archive created:** commit `d0e7dbbe8bffdd7336be5e7e24b34ab53cff282b`.
- **Resolver Reach context policy updated:** commit `e2e8686b3584dfd49b090d2962e6d2419b99b774`.
- **Resolver Reach development log updated:** initial policy/task entry recorded in commit `82919e883442ae0974800c0f6f44bd91a62754b9`; this is to be updated with final verification after all writes.
- **Resolver Reach README linked to the archive:** commit `b030d882d2f9f9ee88700355d2bc7521d56e09d0`.
- **Wand-Enhancer public AI instructions updated:** commit `a44a0e5daa8746e48433b7e42b085c980cf984eb`. It tells future agents to keep prompt text private and restore context from the central archive.
- **Wand-Enhancer public-safe work log created:** commit `67983fe5c4bc309b0f67a05bca9d57adad126460`.
- **Verification:** all six GitHub write operations returned successful commit SHAs. Follow-up read-back verification is still required. No build or runtime tests were run; these are documentation/policy-only changes.
- **Important limitation:** this creates a documented cross-session capture/restore procedure, not a ChatGPT-wide event hook. It cannot intercept messages in conversations that do not load the policy or lack private-repository access.


### Prompt 006 — Mandatory version bumps and repository knowledge persistence
- Date: 2026-10-22 (America/Los_Angeles)
- Target: all repositories accessible to the connected GitHub account
- Exact prompt:

> always add a new version to softwares you update and always saves you knowledge to id
