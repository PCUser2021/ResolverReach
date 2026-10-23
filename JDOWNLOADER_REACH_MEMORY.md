# JDownloader Reach — durable project memory
Snapshot: 2026-10-23

## Location and branches
- Repository: https://github.com/PCUser2021/ResolverReach (metadata currently says private).
- Project branch: https://github.com/PCUser2021/ResolverReach/tree/feature/jdownloader-reach-prototype
- Full pre-removal backup: `backup/JDownloaderReach-full-2026-10-23`, based on commit `14b1bb89bd4366c1ed5df60f043726ec7a48392f`. It preserves the previous integrated app, installers, workflow, docs, and Git history.
- Keep this prototype branch separate from `main` unless the user asks for a merge.
- Related research repositories: https://github.com/mycodedoesnotcompile2/jdownloader_mirror and https://github.com/PCUser2021/ResolverReach.

## Product and current scope
JDownloader Reach 0.3.0 is an independent Windows x64/ARM64 desktop control shell written in C#/.NET 10 with Avalonia and FluentAvalonia. It does not port JDownloader's Java engine and is not a complete replacement for the original UI.

Current source:
- Project: `src/JDownloaderReach/JDownloaderReach.csproj`
- UI: `src/JDownloaderReach/MainWindow.axaml`, `MainWindow.axaml.cs`
- API wrapper: `src/JDownloaderReach/JDownloaderLocalApiClient.cs`
- Build: `.github/workflows/jdownloader-reach.yml`
- Installers: `installer/JDownloaderReach-x64.iss`, `installer/JDownloaderReach-arm64.iss`
- Docs: `src/JDownloaderReach/README.md`, `src/JDownloaderReach/CHANGELOG.md`, `docs/JDownloaderReach-Integration-Architecture.md`

Behavior: find/launch a separately installed `JDownloader2.exe`; check optional API state; submit pasted links to LinkGrabber; request a read-only download-package snapshot; request queued downloads start; launch Resolver Reach; save executable paths under `%LOCALAPPDATA%\JDownloaderReach\settings.json`; export activity/settings/runtime info to a ZIP via Save Logs.

## Explicit removal in v0.3.0
The Ollama GGUF Importer is no longer integrated into JDownloader Reach: no UI card, path detection or saved setting, browse/launch handlers, installer shortcut, binary payload, workflow restore/publish step, or PE validation. The separate `src/OllamaGgufImporter/` application and `.github/workflows/ollama-gguf-importer.yml` remain independent and untouched.

Both architecture installers should bundle only JDownloader Reach and Resolver Reach. The shell's own product version, assembly version, file version, UI version label, workflow publish version and installer metadata must stay synchronized at 0.3.0 / 0.3.0.0. Build both `win-x64` and `win-arm64`; verify PE machine types and Inno Setup compilation in Actions before claiming installers are ready.

## Security and licensing
- API address fixed to `http://127.0.0.1:3128/`. The legacy API may be deprecated and unauthenticated; never expose port 3128 to LAN/internet.
- No JDownloader engine is bundled. Prototype actions still need validation against a real running JDownloader instance.
- JDownloader source headers were identified as GPL-3.0-or-later; this shell did not copy the Java implementation. Resolver Reach has MIT licensing and applicable third-party notices. Audit licensing before any future engine-source/binary redistribution.

## Previous validated baseline (0.2.0, importer included)
Successful workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39943711695, commit `8a2feed6059ee3fc8d6590f5b679e26a79a8b711`. Both architecture jobs succeeded, including PE checks, installer compilation, and artifact upload. These results apply only to the previous three-app bundle.
- x64 artifact ID `12496179768`; extracted setup SHA-256 `5b865279309ccc790f80be05fa92cd3e177c0a7736f416b431d27fa7e442bc8f`.
- ARM64 artifact ID `12497252555`; extracted setup SHA-256 `d2d4a1a6309038fbd7ba76324d6132c1b455ce9da7c3ffe25334e0ef2ced5f48`.
- Actions artifacts have limited retention and are not permanent release assets. No physical-device GUI/API test was performed.

## Backup, memory and public-page limits
- The prompt and work records are in `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, `DEVELOPMENT_LOG.md`, and `BACKUP_MANIFEST.md`. Keep the prompt archive private.
- This is a project-specific ChatGPT context reconstruction, not a complete account-wide ChatGPT Memory export.
- The repository is private and holds unrelated apps plus verbatim prompts. Do not expose the whole repository just to publish JDownloader Reach. The available GitHub connector can read and write files/branches but does not offer repository creation or visibility changes. A dedicated clean public repository should be created and then its public visibility verified.
- The new 0.3.0 workflow must be checked after the change. Do not infer build success from the earlier v0.2.0 run.
