# Resolver Reach project backup manifest

Snapshot date: 2026-10-23  
Primary repository: https://github.com/PCUser2021/ResolverReach  
Visibility at last check: **private; the user requested public access, but the owner must still change the GitHub visibility setting**  
Product version: **Resolver Reach 2.1.1**  
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
