# Resolver Reach project backup manifest

Snapshot date: 2026-10-23  
Primary repository: https://github.com/PCUser2021/ResolverReach  
Visibility at last check: **private; the user requested public access, but the owner must still change the GitHub visibility setting**  
Product version: **Resolver Reach 2.1.0**  
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


## JDownloader Reach full project backup — 2026-10-23

- **Pre-removal backup branch:** `backup/JDownloaderReach-full-2026-10-23` was created before the 0.3.0 cleanup. Its base commit is `14b1bb89bd4366c1ed5df60f043726ec7a48392f`, which was the head of `feature/jdownloader-reach-prototype` at the time. It preserves the original full repository tree/history, installer scripts, three-app workflow, integration source, docs, licenses and project records.
- **Active cleaned branch:** `feature/jdownloader-reach-prototype` is the working line for JDownloader Reach 0.3.0 without the Ollama GGUF Importer integration. The standalone importer application and independent build remain in the repository. The latest cleanup snapshot branch `backup/JDownloaderReach-0.3.0-clean-2026-10-23-r2` includes the PowerShell PE-check syntax fix at commit `b86b6743c1d8da8e597bd9e3cbf10dcecfe35305`.
- **Project-specific memory:** `JDOWNLOADER_REACH_MEMORY.md` is the durable scoped memory backup; `PROMPT_ARCHIVE.md` remains private and contains exact repository-related prompts.
- **Existing CI installer artifacts:** x64 artifact ID `12496179768` and ARM64 artifact ID `12497252555` from run `39943711695` were verified for the 0.2.0 three-app bundle, not the cleaned 0.3.0 bundle. GitHub Actions artifacts are temporary (30-day retention); they are not a permanent source-tree backup.
- **Public-page status:** repository visibility is private. The connected GitHub actions do not offer repository creation or visibility mutation. Do not expose the central repository's full prompt archive merely to publish the one project; publish to a dedicated repository and verify its visibility.


## JDownloader Reach 0.3.0 clean installers — verified (2026-10-23)

- Successful workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39946624955, commit `b86b6743c1d8da8e597bd9e3cbf10dcecfe35305`; both target architecture jobs passed.
- x64: Actions artifact `12498472300`; ZIP size 64,495,157 bytes; extracted EXE SHA-256 `4aee55e9eef29647435dc9b4492c182c9fe4ec752a8b612584177d33d7cb6d02`.
- ARM64: Actions artifact `12497973329`; ZIP size 59,116,477 bytes; extracted EXE SHA-256 `8538a3a53f6b73811434a2b631271bd9c8e726a82fcdd26b698bb468c0a5e57a`.
- Both downloaded artifact ZIP archives passed integrity validation. Artifact retention expires 2026-11-22. The permanent source backup is the Git branch snapshot, not the Actions artifacts.
- These version 0.3.0 installers contain JDownloader Reach and Resolver Reach only. No physical-device or live JDownloader API interaction test was performed.
