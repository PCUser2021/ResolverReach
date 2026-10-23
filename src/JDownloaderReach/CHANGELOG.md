# JDownloader Reach changelog

## 0.2.0 — 2026-10-23 — x64 and ARM64 installers

### Added
- Added native `win-x64` and `win-arm64` publishing for JDownloader Reach, Resolver Reach, and Ollama GGUF Importer.
- Added separate Windows installers, `JDownloaderReach-Setup-x64.exe` and `JDownloaderReach-Setup-arm64.exe`, with OS-architecture guards and Start Menu shortcuts for all three applications.
- Added CI validation of each published executable's PE machine type before installer compilation.
- Added the license and third-party notice documents to each installer payload.

### Validation
- Windows CI must complete both matrix jobs successfully before either architecture is described as verified. The running workflow is linked from the repository Actions tab.


## 0.1.0 — 2026-10-23 — prototype

### Added
- Created an Avalonia/.NET 10 Windows x64 control shell.
- Added a local-only client for JDownloader's deprecated local API: state check, LinkGrabber link submission, download-package snapshot, and start-downloads request.
- Added executable path selection and process launching for JDownloader, Resolver Reach, and Ollama GGUF Importer.
- Added persistent local executable-path settings and Save Logs ZIP export.
- Added a Windows x64 workflow to publish the shell and both existing applications into a ZIP artifact.

### Limitations
- JDownloader's Java engine is not bundled or converted.
- The local API was not verified against a running JDownloader instance in this environment.
- A local build and interactive GUI/API test were not performed during authoring; consult the associated GitHub Actions run for CI results.
