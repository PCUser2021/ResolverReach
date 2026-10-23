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


## 2026-10-22 — Adopt mandatory version bumps and knowledge persistence policy

- **User request (archived privately):** “always add a new version to softwares you update and always saves you knowledge to id”
- Updated the private central archive first: `PROMPT_ARCHIVE.md` commit `7b48a65d982b5b54e4bed9e8247d2402713f7f48`.
- Added an explicit policy to `PROJECT_CONTEXT.md` requiring a product version bump for each shipped software update, synchronized version metadata, a changelog entry, verification of the actual artifact, and persistence of durable knowledge in repository records. Documentation-only changes do not bump the application's binary version.
- Updated the public Wand-Enhancer `AGENTS.md` with the same versioning and knowledge-preservation rules. Raw user prompts remain in the private archive and must never be copied into the public repo.
- No application source code or shipped product behavior changed in this task, so no application version was incremented and no product changelog release entry was added.
- **Validation still pending:** verify GitHub write commits and read back the updated files. No application build or runtime tests are applicable to this documentation-only policy change.


### Verification follow-up — 2026-10-22
- GitHub read-back confirmed the mandatory versioning/knowledge-persistence section is present in `PROJECT_CONTEXT.md` (blob `5ce54854c01b54f45800b136d7941845adf51f28`).
- GitHub read-back confirmed the exact user prompt is present in private `PROMPT_ARCHIVE.md` (blob `b61567b9501dc0bc256e9b299539f57325aac56f`).
- GitHub read-back confirmed the versioning rule and knowledge-persistence rule are present in public `Wand-Enhancer/AGENTS.md` (blob `6dc15b096486ff3720682bb8bb272e0ed872e7b6`).
- Public-safe work-log policy is present in `Wand-Enhancer/AI_WORK_LOG.md` (blob `0563d0275edf0f5c11552a6c941cabcadfab5af8`).
- No product code changed; application versions remain unchanged in this documentation-only task. No build or runtime tests were run.


## 2026-10-23 — Resolver Reach 2.0.0 single-window UI replacement

- **User request:** Replace Resolver Reach's GUI using the supplied OnionFruitt screenshot as a visual reference; expose only live cURL resolver testing and paste-links testing; show all results and progress in the one main window.
- **Design:** Dark slate surfaces, a prominent red status strip, rounded cards, and restrained pink accents. No screenshot sidebar/settings navigation, because the user explicitly asked for exactly two visible testing paths and no extra UI.
- **UI changes:** Replaced `src/ResolverReach/MainView.axaml`; simplified `src/ResolverReach/MainView.axaml.cs` to initialization and Escape-to-cancel; set a fixed dark palette in `src/ResolverReach/App.axaml`. Removed visible theme selection, Paste Links menu, Save Logs button, and save-file-picker flow.
- **Resolver behavior preserved:** Live cURL wiki catalog; pasted URL input; RFC 8484 POST with GET fallback; DNS-response validation; HTTP/latency/DNS details; bounded concurrency; 512-endpoint limit; in-window progress and Escape cancellation.
- **Version:** Advanced Resolver Reach from 1.0.0 to 2.0.0; synchronized project/assembly/file metadata, HTTP User-Agent strings, workflow publish version, installer version, README, and changelog.
- **Changed files:** `src/ResolverReach/App.axaml`, `src/ResolverReach/MainView.axaml`, `src/ResolverReach/MainView.axaml.cs`, `src/ResolverReach/ResolverReachViewModel.cs`, `src/ResolverReach/CurlWikiResolverCatalog.cs`, `src/ResolverReach/ResolverReach.csproj`, `.github/workflows/windows-build.yml`, `installer/ResolverReach.iss`, `README.md`, `CHANGELOG.md`, `PROJECT_CONTEXT.md`, and this log.
- **Validation state at commit time:** Source changes are committed to a feature branch. No local Windows build was run in this environment. Record the actual GitHub Actions build result and artifact status in a follow-up entry; do not infer success from an opened pull request alone.


### Verification follow-up — Resolver Reach 2.0.0 main-branch build (2026-10-23)
- Merged implementation commit: `35ff1517f7e0c0ce42fe1a8500935123d922dd30` (PR [#1](https://github.com/PCUser2021/ResolverReach/pull/1)).
- Successful Windows x64 workflow: [run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989), commit `35ff1517f7e0c0ce42fe1a8500935123d922dd30`; conclusion `success`.
- Verified successful steps: .NET 10 setup, restore, self-contained x64 publish, Inno Setup availability/install, installer compile, and upload of `ResolverReach-Windows-x64`.
- Artifact ID `12490750995`, archive size 33,475,336 bytes, not expired at verification; listed expiry 2026-11-06. The archive was downloaded and inspected; it contains `ResolverReach-Setup-x64.exe`.
- Installer copied for direct download: `/mnt/data/Resolver-Reach-2.0.0-Setup-x64.exe`, size 33,971,274 bytes. Installer string inspection verified `Resolver Reach Setup`, `Resolver Reach`, and `2.0.0`. SHA-256: `0023ab33844ba6ffe00f572eb0ed28e395b3e46044fb392f893726f20df2bc22`.
- Static source read-back confirmed exactly two visible UI buttons, no old theme/menu/Save Logs controls, all results/status/progress in the one window, Escape cancellation preserved, and project version metadata at 2.0.0/2.0.0.0/2.0.0.0.
- No endpoint runtime DNS requests or GUI-interaction tests were run in CI; no claim is made that every public resolver was reachable at build time.


## 2026-10-23 — Public-distribution request and backup preparation

- **User intent:** Make Resolver Reach discoverable on GitHub, retain the explicit personal/home-use limitation, provide an app icon and screenshot, and persist a full project backup including available ChatGPT-derived project knowledge.
- **Security decision:** The existing `PCUser2021/ResolverReach` repository remains private. It contains implementation source for Resolver Reach and Ollama GGUF Importer. GitHub's visibility documentation states that public visibility exposes the code to everyone and allows forks, so exposing this repository would violate the user's requirement to withhold source.
- **Tooling limitation:** The current connected GitHub actions provide source/file/history writes and repository metadata reads but no operation to create a new repository or change visibility. Accordingly, a separate public repository has not been created and the existing repository was not made public. Do not imply otherwise.
- **Prepared distribution package:** `/mnt/data/ResolverReach-Public-Distribution-Kit.zip`; SHA-256 `0295e351aa532b70f8e84d83054d0b9e78a9f31301a61b70008188f2c0a9030b`. Contains the v2.0.0 installer, public README, repository-description text (256 characters), proprietary personal/home-use license terms, icon artwork and a labelled interface preview; no source files.
- **Private repo draft assets/records:** `public-distribution/README.md`, `public-distribution/LICENSE.txt`, `public-distribution/REPO_DESCRIPTION.txt`, `public-distribution/assets/resolverreach-icon.svg`, `public-distribution/assets/resolverreach-preview.svg`, `PUBLIC_DISTRIBUTION_PLAN.md`, and `BACKUP_MANIFEST.md`.
- **Screenshot limitation:** No Windows GUI screen capture was performed. The generated preview is a design illustration rendered to match the actual v2.0.0 XAML layout, and is clearly labelled as a preview rather than a capture. Replace it with a real screenshot before making a public page claim that a screenshot was captured.
- **License caveat:** Requested personal/home-use restriction and reverse-engineering prohibition are included in the drafts. Their enforceability cannot be guaranteed against non-waivable rights under every jurisdiction.
- **Backup state:** Source plus Git history remains in the private repo. Prompt 008 was archived before work; this outcome and updated project context are now being saved. A separate full export of all ChatGPT platform Memory/historical chats is not supported by available tools; repository-related knowledge is preserved in the private prompt archive, context, and development log.
- **Next step:** create a separate public binary-only repository (suggested name `ResolverReach-Downloads`) and upload the prepared kit, keeping the source repository private.


### Final backup and publication status — 2026-10-23
- Exact request archived as Prompt 008; outcome recorded in `PROMPT_ARCHIVE.md`.
- Public distribution drafts and icon/preview source are committed under `public-distribution/` in the **private** source repository. `PUBLIC_DISTRIBUTION_PLAN.md` and `BACKUP_MANIFEST.md` explain the safe release-only repository design.
- Local publication package: `/mnt/data/ResolverReach-Public-Distribution-Kit.zip`; SHA-256 `0295e351aa532b70f8e84d83054d0b9e78a9f31301a61b70008188f2c0a9030b`. It includes the installer, public-facing docs, license terms, icon artwork, and a clearly disclosed layout preview, not source.
- Do not make the current repository public: it contains application source for two projects. GitHub public visibility exposes tracked code and commit history. This session's available GitHub connection does not expose repository-creation or visibility-change operations, so no new public repository was created and the existing repository remains private.
- The preview image is a design illustration rendered from the XAML layout, not a screenshot taken from the running app. No genuine screen capture has been claimed.
- Created stable snapshot branch `backup/ResolverReach-2.0.0-2026-10-23`. This final audit commit is then synchronized to that branch; final read-back records whether the heads match.
- A complete export of ChatGPT platform Memory/historic chat data cannot be produced through the connected tools. All repository-related prompts, implementation decisions, current source state, build verification and handoff limitations visible in this work are kept in the private repository's archive/context/log.


## 2026-10-23 — Prepare Resolver Reach source repository for public visibility

- **User authorization:** Prompt 009 explicitly approves publishing the source code and the whole repository publicly, overriding the earlier request to keep source code inaccessible.
- **Front page:** Replaced root `README.md` with a public-facing app description, icon artwork, a clearly disclosed UI design preview, feature explanation, build verification link, license summary, and repository visibility instructions. Main README commit `a2eee58f267d2cd72c5a4464e25d37431c0db97c`; visibility wording update `9c590b4248ad2a6e3cd1f009a9b6e349c6d52dba`.
- **License:** Added root `LICENSE.txt` with personal/home-use restrictions and a frank caveat that making the repository public inherently exposes source files and allows forks; a license cannot technically block access to public content. Commit `6554a8d90c70658344f7c3799188c962bedc32d1`.
- **Description:** The requested 256-character summary is stored in `public-distribution/REPO_DESCRIPTION.txt`; GitHub's About metadata was not changed because no repository-settings/write action is exposed in the connected GitHub toolset.
- **Icon/preview:** Root README displays `public-distribution/assets/resolverreach-icon.svg` and `public-distribution/assets/resolverreach-preview.svg`. The preview is illustrative, not an actual screenshot capture.
- **Visibility blocker:** No GitHub action for changing repository visibility exists in this session and `gh` CLI is unavailable/unauthenticated in the container. Last metadata read still reports `private`. The source repository has not been made public; the repository owner must complete Settings → Danger Zone → Change repository visibility → Public and then verify metadata.
- **Consequence:** Once made public, the complete committed history, prompt archive, project records, source for both Resolver Reach and Ollama GGUF Importer, and GitHub Actions logs become visible. The user has approved public source visibility.
- **Memory limitation:** Complete platform-level ChatGPT Memory and all historic chats cannot be exported by available tools. Relevant repository prompts, project decisions, source state, backup branch and build validation remain in Git.


## 2026-10-23 — MIT licensing and release download automation (completed)

- **User intent:** Allow anyone to use Resolver Reach and the project source for any purpose and make permanent installer links available through GitHub Releases.
- **License:** Replaced personal/home-use-only restrictions with MIT in the root license files and public distribution draft. The README states that original project code/docs/assets may be used for any purpose subject to retaining the MIT notice. Third-party dependencies retain their own licenses.
- **Download UX:** Added stable README link `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe` and link to all releases.
- **Release automation in this task:** Add a `publish-release` job with `contents: write` scoped to that job. After the Windows x64 build/artifact succeeds, the job is gated to a designated main commit containing `[publish-release]`, downloads `ResolverReach-Windows-x64`, creates the `v2.0.0` release, and attaches `ResolverReach-Setup-x64.exe`.
- **Pre-change facts:** Releases API returned an empty list and the `v2.0.0` tag did not exist. Final status must be verified from the ensuing CI run and Release API response.
- **Visibility status:** Repository metadata currently reports `private`. The available GitHub connector does not provide a repository visibility mutation, so public access needs a manual GitHub Settings change.
- **Version policy:** No product behavior/source-code logic changed; product version remains 2.0.0. This is a license/docs/release-pipeline update.
- **No live DNS test claim:** Packaging success does not mean every DoH endpoint has been runtime-tested.


## 2026-10-23 — MIT licensing and release download automation (completed)

- **User intent:** Allow anyone to use Resolver Reach and the project source for any purpose and make permanent installer links available through GitHub Releases.
- **License:** Replaced personal/home-use-only restrictions with MIT in the root license files and public distribution draft. The README states that original project code/docs/assets may be used for any purpose subject to retaining the MIT notice. Third-party dependencies retain their own license terms.
- **Download UX:** Added stable README link `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe` and link to all releases.
- **Release automation:** The workflow release job is being added with `contents: write` scoped only to that job. After the Windows x64 build/artifact succeeds, the job will be gated to a designated main commit containing `[publish-release]`, download `ResolverReach-Windows-x64`, create the `v2.0.0` release, and attach `ResolverReach-Setup-x64.exe`.
- **Pre-change facts:** Releases API returned an empty list and the `v2.0.0` tag did not exist. Final status must be verified from the ensuing CI run and Release API response.
- **Visibility status:** Repository metadata currently reports `private`. The available GitHub connector does not provide a repository visibility mutation, so public access needs a manual GitHub Settings change.
- **Version policy:** No product behavior/source-code logic changed; product version remains 2.0.0. This is a license/docs/release-pipeline update.
- **No live DNS test claim:** Packaging success does not mean every DoH endpoint has been runtime-tested.


### Verification follow-up — Release published successfully (2026-10-23)
- Release workflow: [run #39933931024](https://github.com/PCUser2021/ResolverReach/actions/runs/39933931024), commit `6889479639a97ece67ed37108de44022fa59d2f5`; final run status `completed`, conclusion `success`.
- Windows build steps passed: .NET setup, restore, self-contained x64 publish, Inno Setup, installer compilation, and artifact upload.
- Release job passed: downloaded the `ResolverReach-Windows-x64` artifact and published GitHub Release [v2.0.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.0.0).
- Verified release is published (not a draft) with uploaded asset `ResolverReach-Setup-x64.exe`, 33,975,898 bytes. Browser URL: `https://github.com/PCUser2021/ResolverReach/releases/download/v2.0.0/ResolverReach-Setup-x64.exe`.
- Stable latest URL in README: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- Repository metadata still reports `private`; the release asset exists but public anonymous downloads/source browsing still require the GitHub repository Visibility setting to be changed to Public.
- App version remains 2.0.0; only license/docs/release automation changed. CI did not run live DNS reachability tests against every endpoint.


## 2026-10-23 — Resolver Reach 2.1.0 titlebar fix and ZIP Save Logs

- **User request:** Fix the screenshot-highlighted title/version overlap in the top-right Windows caption area and add a Save Logs action that lets the user choose a destination and saves full logs as ZIP.
- **Version:** Bumped Resolver Reach to **2.1.0**, assembly/file version **2.1.0.0**, installer version **2.1.0**. Updated current README/changelog/project context/distribution notes and release workflow to tag `v2.1.0`.
- **UI fix:** `src/ResolverReach/MainView.axaml` now reserves 190 DIPs at the right edge of the extended header, keeping the version badge clear of the Windows minimize/maximize/close controls. Added a visible `Save Logs` button in the results-header row.
- **Save dialog:** `src/ResolverReach/MainView.axaml.cs` uses Avalonia `StorageProvider.SaveFilePickerAsync` with a ZIP filter and a timestamped suggested filename. Cancel is handled without writing an archive; success/failure is reflected in the main window status.
- **Full session ZIP:** `src/ResolverReach/ResolverReachViewModel.cs` maintains a timestamped activity log and historical endpoint result rows across scans during the current app session. ZIP entries: `session-summary.txt`, `activity.log`, `resolver-results-history.csv`, `current-visible-results.csv`, and `session.json`. Entries include run mode, attempted endpoints, per-endpoint status/HTTP code/latency/DNS result and details, final scan summaries, cancellation/errors, product/runtime/session metadata, and current UI status.
- **Build/release verification:** [GitHub Actions run #39937042568](https://github.com/PCUser2021/ResolverReach/actions/runs/39937042568) completed with conclusion `success`. .NET restore, self-contained win-x64 publish, Inno Setup compile, artifact upload, and release publish all succeeded.
- **Published artifact:** [Resolver Reach v2.1.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.0), asset `ResolverReach-Setup-x64.exe`, 33,980,655 bytes. Stable download: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- **Testing limitation:** No interactive GUI test was run on a live Windows desktop in this task; CI validates compilation/packaging and asset publishing, not a visual caption-area inspection or click-through test of ZIP creation.
- **Visibility:** Repository metadata still reports `private`; the release asset is hosted but downloads/source browsing remain limited to authorized users until repository visibility is made public by the owner.


## 2026-10-23 — Analyze uploaded DoH log and implement 2.1.1 reliability fixes

- **Input:** `ResolverReach-Logs-20261023-032704.zip`, Windows 11 build 26200, .NET runtime 10.0.13, Resolver Reach 2.1.0.
- **Evidence:** 266 endpoints completed: 235 PASS, 1 DNS ERROR (valid RCODE REFUSED), 30 FAIL. Valid DoH response percentage was 88.7%; the claim that every endpoint was unreachable did not match the saved scan summary.
- **Failure breakdown:** 11 ~5-second timeouts; 11 HTTPS/TLS/HTTP/2 failures; 4 HTTP status errors; 3 HTTP 200 responses with a mismatched transaction ID; 1 HTML response instead of application/dns-message.
- **Source root cause:** 5-second `SocketsHttpHandler.ConnectTimeout`, 10-second `HttpClient.Timeout`, no HTTP/1.1 fallback for HTTP/2 protocol resets, and eight concurrent probes.
- **Fixes committed in 2.1.1:** connect timeout 12s, request timeout 20s, concurrency 6, one retry on client timeout, one retry after transient HTTP status 408/425/429/500/502/503/504, exact HTTP/1.1 fallback after detected HTTP/2 protocol errors.
- **Result-summary accuracy:** scan completion now separates transport failures (no HTTP response) from HTTP responses that failed DoH validation. DNS RCODE REFUSED is still a valid exchange marked DNS ERROR; dead/nonexistent hostname, HTTP denial/server errors, wrong MIME type, and invalid DNS IDs remain failures.
- **Version:** project 2.1.1, assembly/file 2.1.1.0, installer 2.1.1, release tag v2.1.1. App behavior and failure classification are the reason for the patch version bump.
- **CI/release:** implementation commit `766b1b6bd2bca0dcd3e01b7b4d4e9ff32e1c833d` triggered [Windows build #39940585354](https://github.com/PCUser2021/ResolverReach/actions/runs/39940585354) and release job on the same workflow run. Build/release result is pending at log creation time.
- **Testing limitation:** a successful build will not prove all 266 endpoints are live. A fresh post-fix scan from the user's VPN/network is required to measure improvement; third-party endpoints may remain offline or non-compliant.


### Verification follow-up — Resolver Reach 2.1.1 build and release complete (2026-10-23)
- Windows x64 build and installer job: [workflow run #39940585354](https://github.com/PCUser2021/ResolverReach/actions/runs/39940585354), completed successfully.
- Verified steps: .NET 10 setup, restore, self-contained Windows x64 publish, Inno Setup installation, installer compilation, and artifact upload.
- Release job also completed successfully. Published Release: [Resolver Reach v2.1.1](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.1).
- Asset: `ResolverReach-Setup-x64.exe`, 33,979,934 bytes. Direct URL: `https://github.com/PCUser2021/ResolverReach/releases/download/v2.1.1/ResolverReach-Setup-x64.exe`; stable link: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- The 2.1.1 source commit is `766b1b6bd2bca0dcd3e01b7b4d4e9ff32e1c833d`; documentation and archive verification followed on main.
- Code fixes: 12s connect timeout, 20s request timeout, six concurrent probes, one retry for timeouts/transient HTTP statuses, and HTTP/1.1 fallback after HTTP/2 protocol errors. Summary wording now distinguishes transport errors from HTTP replies failing DNS validation.
- Important limit: build and release pass does not demonstrate a post-fix scan improvement. The user needs to run v2.1.1 and compare a new scan; unreachable/misconfigured third-party resolvers will still fail.
- Last repository metadata check continued to report private visibility. Release downloads require repository access until visibility is made public.


## 2026-10-23 — Resolver Reach 2.2.0 ARM64 packaging and metallic vector-icon overhaul

- **User request:** Diagnose new run archive `ResolverReach-Logs-20261023-040016.zip`, add an ARM64 installer, and redesign every app icon in an original style inspired by Apple's premium Logic Pro visual design.
- **Log retrieval status:** The supplied MediaFire direct-download URL could not be fetched: browser returned a cache-miss response and the container could not resolve the download host. The ZIP itself was not available, so no new-run failure statistics are asserted here. User needs to upload the ZIP directly into chat for precise post-2.1.1 analysis.
- **Project version:** 2.2.0; assembly/file versions 2.2.0.0; installer/release tag v2.2.0.
- **ARM64:** project runtime identifiers now include `win-x64;win-arm64`; workflow builds both RIDs in separate jobs; installer script takes architecture/name preprocessor parameters. x64-compatible setup can install on Windows 11 ARM64 through x64 emulation; native ARM64 installer is separate.
- **Icon changes:** custom dark-titanium/glass app icon (new `ResolverReach.ico` blob), public app icon SVG, eight supporting SVG design sources, in-XAML vector replacements for old R/↻/◎ glyphs, new DoH check and Save Logs iconography, and PASS/DNS ERROR/FAIL result status visual indicators. Visual vocabulary uses steel edges and cyan/blue/violet/magenta light; it is original artwork, not copied Apple Logic Pro art.
- **Preview:** updated public design illustration's icon art and version text; it remains a layout preview, not a screenshot captured from a running app.
- **Build/release:** pending Windows x64 + ARM64 workflow verification and v2.2.0 dual-asset upload. A successful publish/release run must be read back before claiming downloads available.
- **Live resolver validation:** the earlier 2.1.0 logs showed actual unreachable/non-compliant services and 2.1.1 made retries/protocol changes. Without the new ZIP, do not make additional log-specific network assertions. Post-release, collect the new ZIP by direct chat upload and compare endpoint failure classes.


### Verification follow-up — Resolver Reach 2.2.0 dual-architecture release completed (2026-10-23)
- Workflow #39945785064 completed with conclusion success: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064
- x64 and ARM64 jobs both passed .NET restore, self-contained publish, Inno Setup install, installer compilation, and artifact upload.
- Release job downloaded both artifacts and published Release v2.2.0: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0
- Assets: ResolverReach-Setup-x64.exe (33,974,496 bytes), ResolverReach-Setup-arm64.exe (31,276,362 bytes).
- Stable links use /releases/latest/download/ResolverReach-Setup-x64.exe and /releases/latest/download/ResolverReach-Setup-arm64.exe.
- The UI uses original vector icons in XAML with matching SVG source family and the redesigned executable/installer ICO. This is an original dark-metallic/spectral treatment, not a copy of Logic Pro's icon.
- The new MediaFire log file was not retrievable from the URL. Do not claim its failure contents were analyzed; ask for direct chat upload. Build success does not prove resolver availability and no interactive GUI test or live resolver re-scan was performed.


## 2026-10-23 — Full project-context handoff and latest diagnostic request

- Archived latest prompt as Prompt 014 in `PROMPT_ARCHIVE.md`.
- Added `AI_SESSION_HANDOFF.md`: authoritative project/version/release details, resolver-probe behavior, older analyzed log statistics, missing newer-log status, user preferences, repository workflow and next actions.
- Added `Software_Project_Master_Context.md` as a cross-project handoff memo covering Resolver Reach 2.2.0 and Ollama GGUF Importer.
- Updated root README and project context with the durable context links and latest version state.
- Current Resolver Reach release remains v2.2.0. Successful dual-architecture build/release workflow #39945785064; x64 installer 33,974,496 bytes and native ARM64 installer 31,276,362 bytes.
- Latest requested log `ResolverReach-Logs-20261023-040016.zip` is not present in current conversation attachments and MediaFire retry failed; only the older 03:27 ZIP has been analyzed. Do not infer latest failure counts from old data. Request direct ZIP upload.
- Backup branch must be advanced to the final updated main SHA after documentation commits and verified.
- Limit: GitHub stores source/context/prompts/decisions for these projects, but cannot export the entire account-level ChatGPT memory database, hidden state, or all unrelated conversations. Do not state otherwise.


### Final project backup verification — Prompt 014 (2026-10-23)
- Root README now links the project master context, full AI handoff, project context, prompt archive, development log and backup manifest.
- `AI_SESSION_HANDOFF.md` and `Software_Project_Master_Context.md` preserve current Resolver Reach 2.2.0 details, the verified x64/ARM64 release, v2.1.1 diagnostic evidence, icon/resource map, user preferences, release/version rules and the latest missing-log blocker.
- Latest user request is archived in `PROMPT_ARCHIVE.md`.
- Verified v2.2.0 workflow #39945785064 and assets: x64 installer 33,974,496 bytes, ARM64 installer 31,276,362 bytes.
- Backup branch `backup/ResolverReach-2.2.0-2026-10-23` was previously synchronized to main; after this documentation commit it must be moved to the final main SHA and verified.
- New log 04:00 is absent from the attachment inventory; MediaFire retrieval failed. Do not claim the latest failure modes are known. User needs to upload the ZIP directly.
- GitHub files preserve project-specific context and source, not all account-wide ChatGPT memory or unrelated chats.


### Final project context backup — 2026-10-23
- Added AI_SESSION_HANDOFF.md and Software_Project_Master_Context.md; README links the context and project archives.
- Prompt 014 captured in PROMPT_ARCHIVE.md.
- Verified current Resolver Reach v2.2.0 workflow #39945785064 and both published installers: x64 33,974,496 bytes; native ARM64 31,276,362 bytes.
- The newer 04:00 log has not been received as an attachment and its external link failed. Do not claim its failure categories are known; only the older 03:27 ZIP has been analyzed.
- The versioned backup branch must match main after the remaining documentation updates.
- Repository files preserve project-specific context, not the entire account-level ChatGPT Memory store.


## 2026-10-23 — Prompt 015: newest scan ZIP still lacks binary chunks

- User supplied a manifest naming `ResolverReach-Logs-20261023-044828.zip`, `Total files: 1`, `Chunks: 0`; visible content ends at the ZIP header prefix `PK\x03\x04\x14`.
- Checked the conversation attachment inventory, Library, and `/mnt/data`: the actual 04:48 ZIP is not present. The manifest/signature is not enough to read the ZIP directory or result CSV/log contents.
- No per-endpoint diagnostic conclusion or code change is justified from this message. Earlier 03:27 log remains the only analyzed scan file.
- Request a direct chat file upload of the actual `ResolverReach-Logs-20261023-044828.zip`; after receiving it, inspect all files, classify errors, compare against v2.2.0, and only change the app where evidence supports a root cause.
- Project source/release/archive are retained in the private GitHub repo. Account-wide ChatGPT memory cannot be exported by these GitHub tools.


## 2026-10-23 — Prompt 017: maintenance dependency/API migration
- Inspected Resolver Reach main, current release metadata, project files, workflows, OnionFruit upstream tree, and existing third-party license notices.
- Existing main remains on Resolver Reach 2.2.0. Work is isolated to PR #2 / branch `maintenance/latest-toolchain-onionfruit-review`; no v2.3.0 release was created.
- Updated primary app packages on the maintenance branch to Avalonia 12.1.4 / Avalonia.Desktop 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6 and ReactiveUI.Reactive 26.0.1. Retained DynamicData 9.5.0 and .NET 10.
- CI action references moved to checkout v7, setup-dotnet v6, upload-artifact v7 / download-artifact v8 and softprops/action-gh-release v3.0.3. Installer compiler discovery now checks Inno Setup 7 and 6 paths plus PATH.
- First Windows CI run restored packages successfully but failed compiling due to removed `TitleBarHitTestType` and using the non-compatibility ReactiveUI API with `System.Reactive` schedulers/commands. Removed the obsolete assignment, selected the compatibility package family, added explicit `System.Reactive.Linq` import, and synchronized product-version fallback.
- Next Windows x64/ARM64 run: https://github.com/PCUser2021/ResolverReach/actions/runs/39993023775 (pending at time of note). Ollama GGUF Importer workflow passed on the previous maintenance revision.
- OnionFruit is an upstream design reference and separate Tor application, not a Resolver Reach runtime component. Existing LGPL license/notice remain; no OnionFruit source was imported.
- Release, merge, and backup branch synchronization remain pending successful CI and final ref/asset read-back.

- Final toolchain audit against official GitHub Releases API found upload-artifact v7.0.2 and download-artifact v8.0.2 published 2026-10-07. Workflow majors were advanced to @v7/@v8; validation remains pending for this exact workflow revision.
