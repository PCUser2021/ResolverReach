# Resolver Reach project backup manifest

Snapshot date: 2026-10-23  
Primary repository: https://github.com/PCUser2021/ResolverReach  
Visibility at last check: **private; the user requested public access, but the owner must still change the GitHub visibility setting**  
Product version: **Resolver Reach 2.2.0**  
The source history includes Resolver Reach and Ollama GGUF Importer.

## Authoritative tracked backup

The private Git repository is the authoritative, versioned backup of source and project records. The user has now explicitly authorized making the source repository public; the current connected toolset cannot execute GitHub's repository-visibility setting, so the change remains pending the repository owner's Settings action.

- Main branch backup head: check the current `main` ref before relying on this snapshot.
- Stable backup branch: `backup/ResolverReach-2.0.0-2026-10-23`.
- Resolver Reach 2.0.0 redesign merge: commit `35ff1517f7e0c0ce42fe1a8500935123d922dd30`.
- Successful Windows x64 publish/installer build: [workflow run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989).
- Project source and icon: `src/ResolverReach/`.
- Packaging and CI: `installer/ResolverReach.iss`, `.github/workflows/windows-build.yml`.
- Product documentation and durable working context: `README.md`, `CHANGELOG.md`, `PROJECT_CONTEXT.md`, `DEVELOPMENT_LOG.md`, `PROMPT_ARCHIVE.md`.
- Publication assets/drafts: `public-distribution/`, `PUBLIC_DISTRIBUTION_PLAN.md`, `LICENSE.txt`.

## Verified shipped binary

- Installer file: `Resolver-Reach-2.0.0-Setup-x64.exe`.
- Size: 33,971,274 bytes.
- SHA-256: `0023ab33844ba6ffe00f572eb0ed28e395b3e46044fb392f893726f20df2bc22`.
- GitHub Actions artifact: `ResolverReach-Windows-x64`, artifact ID `12490750995`; the artifact's listed expiry was 2026-11-06.
- Build/installer success does not prove live reachability for each DNS resolver.

## Public page preparation

- Root `README.md` begins with the Resolver Reach icon and a preview of the v2.0.0 UI. The preview is an illustration and **not** a screenshot captured from the running app.
- Root `LICENSE.txt` includes the requested personal/home-use restrictions and explains that public source visibility cannot be blocked by the license wording.
- Proposed GitHub About/Description text is in `public-distribution/REPO_DESCRIPTION.txt`. It must be copied into the GitHub About settings manually because no repository-metadata-write operation is available in this session.
- GitHub visibility remains private until changed in the web UI: https://github.com/PCUser2021/ResolverReach/settings.

## Visibility warning

Once the private source repository becomes public, GitHub makes all committed code visible, allows public forks, and exposes Actions history/logs. The repository's own history also contains repository-related prompt archive entries and development notes. The user has explicitly authorized source publication in Prompt 009.

## ChatGPT memory limitation

The complete ChatGPT platform-level Memory store and every historical chat cannot be exported through the connected tools. Repository-related prompts, decisions, implementation notes, build validation and handoff state are preserved in `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, and `DEVELOPMENT_LOG.md`; that is not a full platform Memory export.


## MIT licensing and permanent Release downloads — 2026-10-23
- Project-owned source, docs, and original assets now use the MIT License; personal/home-only restrictions are removed from root and distribution license/README files.
- Root README includes the stable direct installer link: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- The main build workflow successfully published [Release v2.0.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.0.0) in [run #39933931024](https://github.com/PCUser2021/ResolverReach/actions/runs/39933931024). The uploaded `ResolverReach-Setup-x64.exe` asset is 33,975,898 bytes. Stable direct link: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- Repository metadata last reported `private`; a GitHub Settings visibility change remains necessary before public source and downloads become available to everyone.


## MIT licensing and permanent Release downloads — 2026-10-23
- Project-owned source, docs, and original assets now use the MIT License; personal/home-only restrictions are removed from root and distribution license/README files.
- Root README includes the stable direct installer link: `https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe`.
- The main build workflow is being configured to publish a `v2.0.0` Release after a successful Windows x64 build on a designated `[publish-release]` main-branch commit. Check the Release API and asset URL before claiming the download is active.
- Repository metadata last reported `private`; a GitHub Settings visibility change remains necessary before public source and downloads become available to everyone.


## Verified MIT Release asset — 2026-10-23
- Release: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.0.0
- Successful workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39933931024
- Asset: `ResolverReach-Setup-x64.exe`, 33,975,898 bytes.
- Stable download: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Fixed version download: https://github.com/PCUser2021/ResolverReach/releases/download/v2.0.0/ResolverReach-Setup-x64.exe
- Repository remains private as of final metadata read; public downloads and source browsing require the owner to change repository visibility.


## Resolver Reach 2.1.0 — titlebar fix and ZIP logs
- Version metadata: product `2.1.0`, assembly/file version `2.1.0.0`.
- UI bug fix: header right padding reserves 190 DIPs for Windows caption controls so the version badge no longer overlaps them.
- New Save Logs button opens a ZIP save picker. ZIP file entries: session summary, activity log, historical endpoint-result CSV, current visible-results CSV, and JSON session/runtime metadata.
- Release workflow is configured for release tag `v2.1.0`; build and asset upload must be verified before considering the update shipped.


## Verified Resolver Reach 2.1.0 build and ZIP log feature — 2026-10-23
- Build/release workflow: [#39937042568](https://github.com/PCUser2021/ResolverReach/actions/runs/39937042568), conclusion `success`.
- Published Release: [v2.1.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.0).
- Asset: `ResolverReach-Setup-x64.exe`, 33,980,655 bytes.
- Stable download: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Source change: 190-DIP safe area for titlebar badge and Save Logs native ZIP save picker.
- ZIP entries: session-summary.txt, activity.log, resolver-results-history.csv, current-visible-results.csv, session.json.
- App version: 2.1.0, assembly/file: 2.1.0.0.
- UI compiled, but no live Windows interactive GUI test was performed.
- GitHub repository visibility remains private as of the final metadata check.


## Resolver Reach 2.1.1 — uploaded DoH log root cause and fix
- Log summary: 266 endpoints scanned; 235 PASS, 1 DNS ERROR, 30 FAIL (88.7% valid DoH response).
- Fixes: 12s connect timeout, 20s HttpClient timeout, six concurrent probes, bounded one-retry policy for timeout/transient HTTP statuses, HTTP/1.1 fallback for HTTP/2 protocol resets and clearer final failure breakdown.
- Product/assembly/file versions: 2.1.1 / 2.1.1.0 / 2.1.1.0; installer/release tag v2.1.1.
- Build and release verification pending workflow #39940585354.


## Final verified Resolver Reach 2.1.1 release — 2026-10-23
- CI: [run #39940585354](https://github.com/PCUser2021/ResolverReach/actions/runs/39940585354), conclusion `success`.
- Release: [v2.1.1](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.1).
- Installer asset: `ResolverReach-Setup-x64.exe`, 33,979,934 bytes.
- Direct download: https://github.com/PCUser2021/ResolverReach/releases/download/v2.1.1/ResolverReach-Setup-x64.exe
- Stable latest link: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Release job confirmed the build artifact was uploaded. The actual endpoint availability percentage after the fix remains to be measured in a new scan.
- The repository visibility remains private at the last metadata read.


## Resolver Reach 2.2.0 — dual architecture and icon overhaul (2026-10-23)
- Project supports both `win-x64` and `win-arm64` runtime identifiers.
- Planned release assets: `ResolverReach-Setup-x64.exe` and `ResolverReach-Setup-arm64.exe`.
- Version 2.2.0 / assembly/file 2.2.0.0 / tag v2.2.0.
- New original icon source under `src/ResolverReach/Assets/Icons/*.svg`; XAML draws a consistent metallic/glass icon family; main `ResolverReach.ico` is replaced in the release commit.
- Public UI preview and app SVG updated. It is illustrative artwork, not a genuine captured screenshot.
- New user-supplied MediaFire log ZIP could not be retrieved; its contents have not been analyzed. Direct upload is needed before claiming new-run diagnosis.
- Build and release status pending until x64 + ARM64 jobs and both uploaded assets are verified.


## Verified Resolver Reach 2.2.0 dual architecture release — 2026-10-23
- Workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064 — conclusion success.
- Release: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0
- x64 installer: ResolverReach-Setup-x64.exe, 33,974,496 bytes.
- Native ARM64 installer: ResolverReach-Setup-arm64.exe, 31,276,362 bytes.
- Stable download URLs:
  - https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
  - https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-arm64.exe
- Project version 2.2.0; assembly/file 2.2.0.0; installer/release tag v2.2.0.
- The redesigned original spectral-metal icon family includes new executable/installer ICO, SVG sources under src/ResolverReach/Assets/Icons/, XAML vectors, and matching public icon/preview.
- The newly supplied MediaFire log was not retrieved; direct ZIP upload is necessary for its exact failure analysis.


## Cross-session project memory backup — 2026-10-23
- Canonical source: https://github.com/PCUser2021/ResolverReach
- Added `AI_SESSION_HANDOFF.md` and `Software_Project_Master_Context.md` to preserve current technical context, release links, user preferences, version rules, known test coverage, and unresolved diagnostics for future sessions.
- Prompt 014 and outcome are tracked in `PROMPT_ARCHIVE.md`; ongoing work and verification are tracked in `DEVELOPMENT_LOG.md`.
- Verified current Resolver Reach Release: v2.2.0; workflow #39945785064 succeeded for `win-x64` and `win-arm64`, and both installer assets were uploaded.
- Latest file `ResolverReach-Logs-20261023-040016.zip` is missing from the current attachment inventory; external MediaFire download failed. It remains an explicit diagnostic blocker.
- Backup branch `backup/ResolverReach-2.2.0-2026-10-23` must match final `main` after all handoff/document commits.
- These records are durable project-specific context only, not an export of account-level ChatGPT Memory or all unrelated chats.


## Final context and backup scope — 2026-10-23
- Prompt 014 captured in `PROMPT_ARCHIVE.md`.
- New repository docs: `AI_SESSION_HANDOFF.md` and `Software_Project_Master_Context.md`; root README links all durable records.
- Resolver Reach source, icon assets, workflows, release v2.2.0 and all Git history are stored in this repository. Versioned backup branch: `backup/ResolverReach-2.2.0-2026-10-23`; synchronize it after the final documentation commits.
- Latest verified releases: x64 (33,974,496 bytes) and native ARM64 (31,276,362 bytes), workflow #39945785064 success.
- Only the earlier log `ResolverReach-Logs-20261023-032704.zip` is available in the current conversation. New log `ResolverReach-Logs-20261023-040016.zip` has not been received/retrieved.
- The docs preserve project-related continuity but are not a copy/export of all account-level ChatGPT Memory, unrelated conversations, or hidden model state.


## Project context backup and unresolved diagnostic — 2026-10-23
- Current request archived as Prompt 014 in PROMPT_ARCHIVE.md.
- Durable context files added: AI_SESSION_HANDOFF.md and Software_Project_Master_Context.md; both are linked from README.
- Resolver Reach source, history, icons, workflows and v2.2.0 Release remain in this private repository. Release workflow #39945785064 succeeded for both x64 and native ARM64.
- Assets: ResolverReach-Setup-x64.exe (33,974,496 bytes); ResolverReach-Setup-arm64.exe (31,276,362 bytes).
- The requested newer ZIP ResolverReach-Logs-20261023-040016.zip is not present in the current attachment inventory. Its external MediaFire URL could not be downloaded. Its errors are unknown pending direct upload.
- After these documentation commits, move backup/ResolverReach-2.2.0-2026-10-23 to the final main SHA and verify equality.
- Project-specific context has been backed up in GitHub; this does not export the whole account-level ChatGPT Memory database or unrelated conversations.


## Latest log handoff status — 2026-10-23
- New claimed file: `ResolverReach-Logs-20261023-044828.zip`.
- Only a manifest was received (`Total files: 1`, `Chunks: 0`) and a short ZIP magic-byte prefix; the archive bytes are unavailable.
- No analysis of the 04:48 scan is possible yet. The older 03:27 ZIP is the only actual uploaded Resolver Reach scan log available to this session.
- Prompt 015 captured in `PROMPT_ARCHIVE.md`. Ask for a direct file upload and then document evidence-based findings.
- Keep `backup/ResolverReach-2.2.0-2026-10-23` synced to the latest main commit after this documentation update.


## Latest manifest follow-up — current repository backup verified
- Prompt 015 recorded in the private prompt archive. The actual `ResolverReach-Logs-20261023-044828.zip` bytes were not delivered; message contained a manifest with 0 chunks and a ZIP prefix only.
- Durable AI handoff files and master context are committed. The previous diagnostic ZIP remains the only scan file available for analysis.
- After the Prompt 015 documentation commit, `backup/ResolverReach-2.2.0-2026-10-23` was synced to the exact `main` commit. Verify both refs again if more documentation/source changes are committed.


- Initial restore attempt exposed a dependency constraint: ReactiveUI.Avalonia 12.1.6 requires ReactiveUI >= 26.0.1. The maintenance branch was corrected to ReactiveUI 26.0.1 before the next CI attempt.

## 2026-10-23 maintenance branch snapshot

- Maintenance branch: `maintenance/latest-toolchain-onionfruit-review`.
- Intent: modernize Resolver Reach's NuGet dependencies and GitHub Actions tooling while preserving the main branch until CI validates the migration.
- Dependency targets: Avalonia 12.1.4; FluentAvaloniaUI 3.1.0; ReactiveUI.Avalonia 12.1.6; ReactiveUI 26.0.1; DynamicData remains 9.5.0 pending authoritative evidence of a newer stable release.
- CI action targets: actions/checkout v7; actions/setup-dotnet v6; actions/upload-artifact v6; actions/download-artifact v6; softprops/action-gh-release v3.0.3.
- Runtime: .NET 10 LTS retained. Official Microsoft release page showed .NET SDK 10.0.402 / runtime 10.0.13 on 2026-10-23.
- Installer: workflow now looks for Inno Setup 7 first and Inno Setup 6 second after installing the Chocolatey package; actual installed version depends on the package repository at workflow time.
- Upstream reference: https://github.com/dragonfruitnetwork/onionfruit. The supplied `git clone ...; cd onionfruit` commands only clone and enter the upstream repository; they do not update Resolver Reach automatically. This branch uses it as an upstream reference only and does not merge or copy OnionFruit application code.
- Validation caveat: no local build or package restore was executed from this session. CI must be green before merging. Major Avalonia 12 / FluentAvalonia 3 changes can require source-level migration fixes.
- Backup scope: GitHub repository commits and branches preserve tracked source history. This manifest, project context, handoff, development log, and prompt archive preserve the repository-specific notes currently tracked in Git. They do not constitute a complete export of ChatGPT account Memory, all chat transcripts, untracked local files, or external GitHub Actions artifacts.


## Maintenance dependency review — 2026-10-23 (not yet shipped)
- Active work branch: `maintenance/latest-toolchain-onionfruit-review`; pull request: https://github.com/PCUser2021/ResolverReach/pull/2.
- Main stays at the verified v2.2.0 until this upgrade passes CI. The branch stages app dependencies and Windows build tooling for v2.3.0.
- Dependency set: Avalonia/Avalonia.Desktop 12.1.4; FluentAvaloniaUI 3.1.0; ReactiveUI.Avalonia.Reactive 12.1.6; ReactiveUI.Reactive 26.0.1; DynamicData 9.5.0; .NET 10.
- Initial CI reached compile after successful restore but failed on upstream API breaking changes. Fixes have been committed; latest x64/ARM64 run #39993023775 was pending at archive time.
- Do not treat the maintenance branch as a release. After CI success, read back both jobs and packaged artifacts, then merge/publish only with the intended release procedure. Sync the versioned backup branch to final main only after finalization.

- Follow-up action audit: official release metadata identifies `actions/upload-artifact@v7.0.2` and `actions/download-artifact@v8.0.2` as current on 2026-10-23. Workflow refs use major tags @v7/@v8. Re-run CI against the updated workflow before merge/release.
