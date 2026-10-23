# Development Log

This is an append-only, chronological audit log for the two Windows applications in this repository. Add an entry for every substantive project-related task, including decisions, requirements, bugs, code/UI/assets/build changes, test outcomes, releases, and deferred items. The current source, commit history, workflow logs, and artifacts remain authoritative.

## 2026-10-22 — Establish persistent project context and logging policy

- **Reason:** Preserve project continuity across ChatGPT conversations and maintain a repository-owned record independent of chat memory.
- **Projects covered:** Resolver Reach and Ollama GGUF Importer.
- **Audited source:** Root `README.md`; `src/ResolverReach/ResolverReach.csproj`; `src/OllamaGgufImporter/OllamaGgufImporter.csproj`; `.github/workflows/ollama-gguf-importer.yml`; `src/OllamaGgufImporter/README.md`.
- **Verified versions at time of audit:** Both project files declare version 1.0.0 (assembly/file version 1.0.0.0). Re-check before any future release/version claim.
- **Verified required features in documentation/source configuration:** Both applications display product-version information and include Save Logs functionality. Resolver Reach exports a session summary and endpoint results. Ollama GGUF Importer exports the activity log, application status, and import queue to UTF-8 text.
- **Documentation change:** Add `PROJECT_CONTEXT.md` as the persistent project overview and working agreement; add this log as the chronological record; link both from the root README.
- **Build/test status for this documentation-only task:** Repository files were read through the connected GitHub integration. No local compile, runtime test, or GitHub Actions run was executed as part of this documentation change. Do not interpret this entry as a build-success claim.
- **Future action rule:** Whenever future work changes source, UI, assets, configuration, build, or documentation, append an entry describing exactly what changed and what verification was actually performed before the task is considered complete.


## 2026-10-22 — Add cross-repository prompt archive and recovery policy

- **User request:** “always auto backup every single prompt for all of my repos and auto restore it”
- **Repository inventory:** The connected GitHub integration returned two accessible repositories for this account at the time of inspection: `PCUser2021/ResolverReach` (private, default branch `main`) and `PCUser2021/Wand-Enhancer` (public, default branch `master`).
- **Changes:** Added private `PROMPT_ARCHIVE.md` containing a capture/restore procedure and the repository-related prompts visible in this conversation; updated `PROJECT_CONTEXT.md` with cross-repository rules; will update this file with outcomes and the public repo's `AGENTS.md` with instructions that route exact prompts to the private archive.
- **Privacy:** Exact prompt text is to remain in the private ResolverReach repository. Do not publish raw prompts in Wand-Enhancer because its repository is public. Redact credentials/secrets before logging.
- **Restore behavior:** New repo-related sessions must read the private archive and current target-repository instructions/source to reconstruct context; never silently reset or overwrite code to an older state.
- **Automation limitation:** No ChatGPT message-event hook is available through these tools, so this cannot intercept every message automatically in all possible ChatGPT conversations. This establishes a repository-side protocol for sessions that load the instruction and have GitHub write access; inability to write must be reported rather than concealed.
- **Verification:** Initial archive creation commit: `d0e7dbbe8bffdd7336be5e7e24b34ab53cff282b`. Context policy commit: `e2e8686b3584dfd49b090d2962e6d2419b99b774`. Remaining actions and commit SHAs must be appended after they are completed and verified. No code compilation or runtime tests are applicable to this policy/documentation work.


## 2026-10-22 — Cross-repository prompt backup setup completed and verified

- Added private central `PROMPT_ARCHIVE.md` to archive repository-related user prompts and task outcomes: creation commit `d0e7dbbe8bffdd7336be5e7e24b34ab53cff282b`; final setup/outcome update commit `6fa9363ea3b7bdb0eac013b8586bdadd96f98354`.
- Updated this repository's `PROJECT_CONTEXT.md` with the central archive, prompt-capture policy, context-restoration rules, privacy rules, and limitations: commit `e2e8686b3584dfd49b090d2962e6d2419b99b774`.
- Updated root `README.md` to link the private archive: commit `b030d882d2f9f9ee88700355d2bc7521d56e09d0`.
- Updated public `PCUser2021/Wand-Enhancer/AGENTS.md` with instructions to restore context from the private archive and never expose verbatim user prompts publicly: commit `a44a0e5daa8746e48433b7e42b085c980cf984eb`.
- Created public-safe `PCUser2021/Wand-Enhancer/AI_WORK_LOG.md`: commit `67983fe5c4bc309b0f67a05bca9d57adad126460`.
- All listed writes returned successful GitHub commit SHAs. The archive, context, and instructions should be read back to verify the final committed contents; do not infer success solely from a tool response.
- No code/build/runtime tests were performed because this change only alters documentation and workflow instructions.
- The prompt archive records repository-related prompts from the current conversation and establishes a process for future repository-related prompts. It cannot act as a global ChatGPT message interceptor; each future session must load and follow the instructions and have access to the private repository. Secrets must be redacted.
