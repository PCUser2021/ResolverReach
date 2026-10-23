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


### Outcome 006 — Mandatory versioning and knowledge persistence rules implemented
- Date: 2026-10-22 (America/Los_Angeles)
- Archived the exact user request before policy changes in commit `7b48a65d982b5b54e4bed9e8247d2402713f7f48`.
- Added mandatory shipped-software version bump, synchronized version metadata, changelog, artifact verification, and repository knowledge-persistence rules to private `PROJECT_CONTEXT.md`: commit `255d9d3e195a7d1d93c858ba1e7b75ab47b88cb6`.
- Added matching instructions to public `PCUser2021/Wand-Enhancer/AGENTS.md`: commit `175893576348fb9dfb5cf76880b4e2f2d6ff6472`.
- Logged policy implementation in private `DEVELOPMENT_LOG.md`: commit `d80c65e0f16333d21d4a70afe68441bc2efc28cb`; added read-back verification: commit `fe686d41f8d634a6f15f543c8c7b413a46b3f878`.
- Updated the public-safe `PCUser2021/Wand-Enhancer/AI_WORK_LOG.md`: policy entry commit `b5c64f50f0e470a4581304f34aa10c3a84b52dbe`; verification follow-up commit `9b5c8c6d16b457a8936e8b7c0e6e8a1c04b81c04`.
- Verification: read-back confirmed the new version/knowledge rules in `PROJECT_CONTEXT.md` (blob `5ce54854c01b54f45800b136d7941845adf51f28`), `AGENTS.md` (blob `6dc15b096486ff3720682bb8bb272e0ed872e7b6`), private archive (blob `b61567b9501dc0bc256e9b299539f57325aac56f`), and public work log (blob `0563d0275edf0f5c11552a6c941cabcadfab5af8`). Subsequent verification log entries were committed; final read-back should confirm the latest entries.
- Version rule: each shipped software update that changes code, behavior/UI, dependencies, assets, configuration, packaging or executable output must advance product version and update the changelog. Patch/minor/major should reflect impact; synchronize all version surfaces and verify the actual built artifact before declaring release.
- Knowledge rule: durable requirements, decisions, discoveries, bug causes, failed attempts and unfinished work must be persisted into repository context/logs. Every repository-related prompt should be archived in this private file first; public repositories only receive non-sensitive summaries.
- No application code changed in this task; no product version was bumped and no build/runtime tests were run. This was a documentation/policy change.
- Limitation remains: this repository policy is not a ChatGPT-wide message interception hook and cannot guarantee automatic capture in sessions that never load the policy or lack GitHub access.


### Prompt 007 — Complete single-window Resolver Reach redesign
- Date: 2026-10-23 (user's current timezone: America/New_York)
- Target: `PCUser2021/ResolverReach` — Resolver Reach
- Reference image: user-provided screenshot of OnionFruitt settings/main window (dark slate panels, narrow title bar, strong red connection/status strip, rounded rows, restrained pink accent, left navigation in screenshot).
- Exact prompt:

> Hi legendary ChatGPT:
> Please 100% do what I say about Resolver Reach
> Delete the whole GUI 100% and redesign it 100% from scratch using these settings:
> 100% copy and paste the GUI design from the screenshot and 100% use it.
> Just 2 options:
> 1: Test Curl Resolvers (Test all DNS over HTTPS Resolvers from the https://github.com/curl/curl/wiki/DNS-over-HTTPS 100% check 100% if they are reachable)
> 2: Add the option: Paste links: Test 100% DNS over HTTPS and check 100% if they are reachable) and add the button: Test Resolvers.
> It's just a: 1-window software: no fucking other window or any extra options; just a 1-window option: the test result should be just on the 1 main window. Remember: It's just a 1-window software, and everything should be in the main window: no extra window, option, or anything; everything should be 100% just in the main window.
> Please develop it like you are a 100% postdoc developer.
> Everything should be in the same main window
> Thank you so much, legendary ChatGPT.
- Status at archive time: captured before implementation; outcome to be appended after source changes and verification.


### Outcome 007 — Resolver Reach 2.0.0 redesign completed and verified
- Date: 2026-10-23
- Target: `PCUser2021/ResolverReach`
- Merged PR: [#1 — Resolver Reach 2.0.0 single-window DoH tester redesign](https://github.com/PCUser2021/ResolverReach/pull/1), squash merge commit `35ff1517f7e0c0ce42fe1a8500935123d922dd30`.
- Product version: `2.0.0`; project, assembly, and file version metadata read back as `2.0.0`, `2.0.0.0`, and `2.0.0.0`.
- UI read-back checks: exactly two visible buttons; both requested test commands present; no theme selector, Paste Links menu, Save Logs button, or secondary save-file dialog; results, progress, and status are bound in the main window; Escape cancellation remains.
- Build: [main-branch Windows x64 build and installer run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989) completed with conclusion `success`. .NET restore, self-contained Windows x64 publish, Inno Setup install/compile, and artifact upload all succeeded.
- Artifact: `ResolverReach-Windows-x64`, GitHub artifact ID `12490750995`, 33,475,336-byte archive; non-expired at verification time, scheduled expiry 2026-11-06. Downloaded main-run ZIP was inspected and contained `ResolverReach-Setup-x64.exe`.
- Local installer inspection: `Resolver-Reach-2.0.0-Setup-x64.exe`, 33,971,274 bytes; `strings -el` confirmed installer name/product/version strings including `2.0.0`. SHA-256: `0023ab33844ba6ffe00f572eb0ed28e395b3e46044fb392f893726f20df2bc22`.
- No live DoH endpoint runtime testing was performed by CI; successful build/installer compilation does not prove every resolver reachable. The app performs live endpoint checks when the user runs it.
- Durable implementation and build verification are documented in `DEVELOPMENT_LOG.md` and `PROJECT_CONTEXT.md`. Prompt 007 was captured in the private archive before implementation.


### Prompt 008 — Public Resolver Reach listing without source access
- Date: 2026-10-23
- Target: `PCUser2021/ResolverReach` and a proposed public binary-only distribution repository.
- Exact prompt:

> Thanks a lot.
> Now, do exactly what I say:
> Make the "Resolver Reach" repository public on GitHub.
> Disable access to the source codes.
> Write a compelling description explaining exactly what "Resolver Reach" does.
> Add the "Resolver Reach" icon to the GitHub page.
> Add a screenshot of "Resolver Reach" to the GitHub page.
>
> In the description, be sure to state: "Resolver Reach" is intended solely for personal, home use. Under no circumstances is anyone permitted to use "Resolver Reach" for any purpose other than personal, home use—this includes accessing the source code or reverse engineering it.
>
> 100% add everything you did to Resolver Reach to GitHub, make a full backup of all you did to Resolver Reach, and make a full backup of everything you did, including your ChatGPT memory
- Status at capture time: request archived before changes. Important constraint: a public GitHub source repository inherently exposes its committed source/history; do not expose the private source repo. Try to use a separate public distribution repository if supported.


### Outcome 008 — Public-distribution preparation; private source preserved
- Date: 2026-10-23
- Original source repository `PCUser2021/ResolverReach` was verified **private** with admin permissions for the connected account.
- Safety decision: did not change visibility of the source repository, because it contains source code for both Resolver Reach and Ollama GGUF Importer. GitHub's documented behavior is that making a private repository public exposes its code and allows public forks. This would conflict with the user's simultaneous requirement to disable source access.
- Repository-creation/visibility actions are not available through the current GitHub connection, so a separate public repository could not be created in this session. Do not claim the requested public listing is live.
- Prepared a source-free local publication kit at `/mnt/data/ResolverReach-Public-Distribution-Kit.zip`, SHA-256 `0295e351aa532b70f8e84d83054d0b9e78a9f31301a61b70008188f2c0a9030b`. It contains the installer, public README, repo description, personal/home-use license terms, brand icon artwork and UI preview; no application source is included.
- Prepared public-facing draft materials in the still-private repo: `public-distribution/README.md`, `public-distribution/LICENSE.txt`, `public-distribution/REPO_DESCRIPTION.txt`, `public-distribution/assets/resolverreach-icon.svg`, `public-distribution/assets/resolverreach-preview.svg`, and `PUBLIC_DISTRIBUTION_PLAN.md`. Added `BACKUP_MANIFEST.md`.
- The image asset is an explicitly labelled design preview/illustration created from the v2.0.0 layout, **not a screenshot captured from a running process**. It must not be described as a genuine app screenshot until one is captured on Windows.
- User's requested personal/home-use and anti-reverse-engineering language appears in the license and public README drafts. The drafts also acknowledge that enforceability and mandatory reverse-engineering exceptions vary by jurisdiction.
- Full ChatGPT platform Memory/history export is not available through these tools. The repository preserves repository-related prompts, project decisions, source state and outcomes; it is not a complete export of platform Memory.
- Next safe publishing step: create a separate public, binary-only repository (suggested name `ResolverReach-Downloads`) and upload the kit contents, preferably publishing the EXE as a GitHub Release asset. Keep the source repository private.


### Final verification — backup and public-distribution preparation (2026-10-23)
- Prompt 008 was saved before implementation; the public-distribution outcome has now also been recorded.
- Existing source repository visibility is still `private`; source code was not exposed.
- Prepared local source-free publication ZIP: `ResolverReach-Public-Distribution-Kit.zip`, SHA-256 `0295e351aa532b70f8e84d83054d0b9e78a9f31301a61b70008188f2c0a9030b`. It contains the installer, README, license, repository description, generated brand-icon artwork, and a disclosed design preview; it does not contain application source.
- GitHub private draft files exist under `public-distribution/`; `PUBLIC_DISTRIBUTION_PLAN.md` and `BACKUP_MANIFEST.md` state the visibility conflict, publication path, and Memory export limitation.
- A stable source snapshot branch named `backup/ResolverReach-2.0.0-2026-10-23` was created; after this final audit-only update it will be advanced to the same final commit as `main` and read back.
- Not completed: creation of a separate public repository or visibility change (no such operation is exposed through this session's connected GitHub interface); a genuine screenshot capture from a running Windows GUI (the included preview is illustrative only); complete platform-level ChatGPT Memory export (not available).


### Prompt 009 — Explicit approval to make Resolver Reach source public
- Date: 2026-10-23
- Target: `PCUser2021/ResolverReach`
- Exact prompt:

> ok fine, turn source code public and 100% make the project public

- Status: user explicitly approved public source visibility after being informed that it means source can be read and forked. The connected GitHub toolset in this session lacks a repository-settings/visibility mutation operation. Do not claim the visibility was changed unless verified using GitHub repository metadata.


### Outcome 009 — Public front-page preparation; visibility change still pending
- Date: 2026-10-23
- The user explicitly approved publishing source code after the earlier explanation that this makes the source and Git history publicly visible.
- The current GitHub connector exposes repository reads and content/commit writes, but not a repository visibility-change operation. No authenticated GitHub CLI is installed in the tool environment. Consequently, the repository was **not** made public by this session.
- Root `README.md` was replaced with a public-facing description, resolver feature summary, app icon reference, interface preview, build verification link, and requested personal/home-use statement. Commit `a2eee58f267d2cd72c5a4464e25d37431c0db97c`; visibility instructions follow-up `9c590b4248ad2a6e3cd1f009a9b6e349c6d52dba`.
- Added root `LICENSE.txt` with the requested personal/home-use terms and explicit caveat that public GitHub visibility makes source files readable and allows public forks regardless of license wording. Commit `6554a8d90c70658344f7c3799188c962bedc32d1`.
- Icon and preview in README point to `public-distribution/assets/resolverreach-icon.svg` and `public-distribution/assets/resolverreach-preview.svg`. The latter is a design illustration, not a genuine screenshot from a running Windows app.
- Proposed repository About description remains in `public-distribution/REPO_DESCRIPTION.txt`; it has not been set as GitHub repository metadata because no metadata-write operation is available.
- Updated `PUBLIC_DISTRIBUTION_PLAN.md` and `BACKUP_MANIFEST.md` to reflect current approval and the remaining GitHub Settings operation. The prompt archive and history contain prior repository-related prompts; these will also become public when the repository visibility is changed.
- The repository's visibility was last read as `private`; don't claim public visibility until a fresh GitHub metadata read reports `public`.
- Full platform ChatGPT Memory/export remains unavailable. Project-related knowledge and outcomes are saved in this private repository.


### Prompt 010 — Make Resolver Reach freely reusable and enable release downloads
- Date: 2026-10-23
- Target: `PCUser2021/ResolverReach`
- Exact prompt:

> ok ok. just let anyone use it and let anyone use this project source code for anything
> https://github.com/PCUser2021/ResolverReach
>
> also active the download links in here too
> https://github.com/PCUser2021/ResolverReach/releases

- Intent recorded: replace the earlier personal/home-use restrictions with a permissive license for the project code the owner controls, and make release download assets available if the connected GitHub tools support it. Verify actual repo visibility, license content, and release assets; never claim public visibility or downloadable release assets without read-back.


### Outcome 010 — MIT license and v2.0.0 release verified (2026-10-23)
- Root and distribution licenses are now MIT for original project work; old home-use-only restrictions were removed.
- Stable download link: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Published release: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.0.0
- Successful build and release workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39933931024
- Asset `ResolverReach-Setup-x64.exe` is uploaded (33,975,898 bytes).
- Repository visibility still reports private; public source and anonymous public downloads require the owner to change visibility in GitHub Settings.
- Product stays at version 2.0.0 because application code/behavior was not changed by this licensing/distribution update.


### Prompt 011 — Fix title/version overlap and add ZIP Save Logs
- Date: 2026-10-23
- Target: `PCUser2021/ResolverReach` — Resolver Reach
- Supplied screenshot: `Resolver Reach bug.png`, with a blue highlight around the top-right Windows caption controls overlapping the version badge.
- Exact prompt:

> ChatGPT, please fix this GUI bug (highlighted in blue) and also add a Save Logs button to select a location to save the full logs to. Save it to a ZIP file

- Required work: fix the top-right title/version/system-caption overlap; add a Save Logs main-window button that prompts for a user-selected ZIP path and exports available full run/activity diagnostics; bump product version and synchronize metadata/changelog; verify the Windows build and update durable context.
- Status at archive time: captured before implementation.


### Outcome 011 — Resolver Reach 2.1.0 titlebar fix and ZIP Save Logs verified
- Date: 2026-10-23
- Updated the main header to reserve a 190-DIP safe area for Windows caption controls; added the **Save Logs** button and Avalonia ZIP save picker.
- Added session-level activity and endpoint-result history. ZIP contains `session-summary.txt`, `activity.log`, `resolver-results-history.csv`, `current-visible-results.csv`, and `session.json`.
- Product/project/assembly/file/installer version advanced to `2.1.0` / `2.1.0.0`; changelog, README, project context and workflow synchronized.
- Windows build/release workflow [#39937042568](https://github.com/PCUser2021/ResolverReach/actions/runs/39937042568) completed successfully. Published release [v2.1.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.0), asset `ResolverReach-Setup-x64.exe`, 33,980,655 bytes; verified stable URL `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- CI confirmed compile, packaging, and upload, but no live interactive Windows GUI test was performed. Repository visibility still reads `private`.


### Prompt 012 — Diagnose DoH resolver scan failures and fix reliability bugs
- Date: 2026-10-23
- Target: `PCUser2021/ResolverReach`
- Input log archive: `ResolverReach-Logs-20261023-032704.zip` (Windows 11 build 26200; Resolver Reach 2.1.0).
- Exact prompt:

> All the resolvers are not reachable, even with Proton VPN! 100% scan; analyze the log file to check why they are not reachable,, and fix all bugs like you're a postdoc developer

- First diagnostic findings from the uploaded archive:
  - The run completed all 266 endpoints: 235 PASS, 1 DNS ERROR (valid DoH response with RCODE REFUSED), 30 FAIL.
  - Thus 236/266 (88.7%) returned structurally valid DoH responses; not all resolvers failed.
  - Of the 30 failures: 11 request timeouts, 11 HTTPS/TLS failures, 4 HTTP status failures, 3 invalid DNS replies due mismatched transaction IDs, and 1 HTML response instead of application/dns-message.
  - Multiple timeouts clustered almost exactly at 5,000 ms. Source has SocketsHttpHandler.ConnectTimeout=5 seconds, HttpClient.Timeout=10 seconds, and sends HTTP/2 with RequestVersionOrLower but has no HTTP/1.1 compatibility retry after HTTP/2 PROTOCOL_ERROR.
  - Some failures appear server-specific (DNS hostname not found, HTTP 403/404/502, wrong MIME type or incorrect DNS ID); no VPN setting can make an offline or non-compliant resolver respond correctly.
- Work requested: improve timeout tolerance, add HTTP/1.1 fallback for HTTP/2 protocol resets, retry transient timeouts/server statuses once, improve status summaries to distinguish transport failures from received-but-invalid HTTP/DoH replies, preserve per-run diagnostics, bump product version, and verify the Windows build and release.
- Status at archive time: log inspected; code changes and build verification follow.


### Prompt 013 — Develop a JDownloader-derived application using ResolverReach
- Date: 2026-10-23
- Target source: `mycodedoesnotcompile2/jdownloader_mirror`
- Target project: `PCUser2021/ResolverReach`
- Exact prompt:

> Develop a new JDownloader from this repo
> [https://github.com/mycodedoesnotcompile2/jdownloader_mirror?utm_source=chatgpt.com](https://github.com/mycodedoesnotcompile2/jdownloader_mirror?utm_source=chatgpt.com)
> And convert it and develop a new JDownloader using 100% of this project
> [https://github.com/PCUser2021/ResolverReach](https://github.com/PCUser2021/ResolverReach)

- Intent: investigate a new download-manager product that reuses the JDownloader source and incorporates all existing ResolverReach-repository applications and functionality.
- Engineering constraints discovered at task start: JDownloader is a large Java codebase; Resolver Reach is a .NET 10/Avalonia application, and the repository also contains the separate Ollama GGUF Importer WinForms app. These technologies cannot be losslessly converted into a single language/framework by a mechanical transformation.
- Workflow: preserve the current `main` branch, work on an isolated feature branch, verify license compatibility before redistribution, and record the actual scope/build results. Any first iteration must clearly distinguish a launcher/prototype from a fully ported and integrated downloader.
- Status at archive time: repository inspection underway; no product code changes made yet.


### Prompt 014 — x64 and ARM64 installers
- Date: 2026-10-23
- Request: Create a Windows installer for both x86-64 and ARM64.
- Requirement: add separate architecture-specific installers for JDownloader Reach together with Resolver Reach and Ollama GGUF Importer. Publish each app for the matching Windows .NET runtime, and reject incompatible native OS architectures in each installer.
- Preserve main and existing app features. Record version changes and actual build/test outcomes in the development log.
- Claim ARM64 support only after the ARM64 build and installer complete successfully in CI.


## 2026-10-23 — JDownloader Reach: remove Ollama integration, preserve full backup, project-memory backup, public page request

### User prompt (verbatim)
> Delete this project; I don't need it
> Ollama GGUF Importer integration
>
> Show me the GitHub page for this project and make it public, save a full backup of everything to GitHub, and also full back up ChatGPT memory
> JDownloader Reach

### Scope interpretation and privacy
- Remove the Ollama GGUF Importer integration from JDownloader Reach itself (UI controls, saved path, launch handlers, installer shortcuts/payload, and JDownloader Reach build workflow). Preserve the existing standalone Ollama GGUF Importer project and its independent build workflow; the request targets its JDownloader Reach integration, not deletion of that separate app.
- Preserve the full pre-removal repository state on branch `backup/JDownloaderReach-full-2026-10-23` before modifying the integration.
- Create a durable project-specific JDownloader Reach memory/context file in the repository. Do not export unrelated general ChatGPT memories or sensitive personal details.
- User requests a public project page. Repository visibility is currently private; do not claim public until GitHub metadata confirms it. The connected GitHub actions available in this session do not expose repository visibility mutation or repository creation, so a manual Settings action may remain necessary.
