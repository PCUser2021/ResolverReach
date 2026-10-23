# JDownloader Reach 0.3.0 — prototype

JDownloader Reach is a Windows x64 and ARM64 Avalonia desktop control shell that connects to an existing JDownloader installation and can launch the separate Resolver Reach DNS-over-HTTPS diagnostic application. Ollama GGUF Importer is not integrated into this app or included in its installers.

## What this prototype does

- Launches a user-selected `JDownloader2.exe`.
- Uses the optional JDownloader local API at the fixed loopback address `http://127.0.0.1:3128/` to check engine state, send links to LinkGrabber, display a read-only download-package snapshot, and request that queued downloads start.
- Launches the existing Resolver Reach application, preserving its RFC 8484 DoH validation and Save Logs ZIP functionality.
- Stores executable paths under `%LOCALAPPDATA%\JDownloaderReach\settings.json`.
- Exports the shell's activity log and runtime/settings metadata to a user-selected ZIP archive.

The repository workflow packages JDownloader Reach and Resolver Reach into architecture-specific Windows setup executables.

## Requirements and setup

- Windows 11 x64 or ARM64.
- A separately installed JDownloader instance. This prototype does **not** bundle the JDownloader Java engine.
- To use queue/link controls, JDownloader's local API must be running and available at `127.0.0.1:3128`. The application does not change JDownloader settings or open/firewall ports.
- If Resolver Reach is present in `tools\ResolverReach`, the app auto-detects it. Otherwise use **Browse…** to select the executable.
- Resolver Reach keeps its own settings and logging; the shell does not merge its session logs. The independent Ollama GGUF Importer remains separate and is not launched or bundled by JDownloader Reach.

## Security note

The source's local API is deprecated and may be unauthenticated. This client hard-codes the destination to `127.0.0.1`; it provides no field for a remote host and does not attempt to expose or enable the API. Only enable the local API if you understand its security implications. Do not expose port 3128 to a LAN or the internet. API requests and response formats may vary across JDownloader builds; the application reports failures rather than pretending a request succeeded.

## What this is not

This release is a functional integration prototype, **not a complete conversion of JDownloader from Java to C#** and not yet a fully unified replacement for the original JDownloader UI. The downloader, host plugins, extraction, accounts, update logic, and link crawling remain inside the existing JDownloader engine. The new app controls a few functions through the local API and can launch the original UI for features not yet exposed by this shell.

A lossless 100% Java-to-C# conversion is not realistically achievable through mechanical conversion. A full native replacement would require a staged rewrite and compatibility work for JDownloader's large plugin/dependency ecosystem. This prototype deliberately preserves the proven engine rather than silently dropping its behavior.

## Build

```text
dotnet restore src/JDownloaderReach/JDownloaderReach.csproj
dotnet publish src/JDownloaderReach/JDownloaderReach.csproj --configuration Release --runtime win-x64 --self-contained true

dotnet publish src/JDownloaderReach/JDownloaderReach.csproj --configuration Release --runtime win-arm64 --self-contained true
```

The repository workflow publishes JDownloader Reach and Resolver Reach as native `win-x64` and `win-arm64` builds, validates each EXE's PE machine type, and compiles `JDownloaderReach-Setup-x64.exe` and `JDownloaderReach-Setup-arm64.exe` installers. CI completion proves compilation and packaging, not live interaction with a running JDownloader instance.

## License and attribution

The new shell is original code in the ResolverReach repository and is covered by the repository's MIT License. The upstream JDownloader source mirror is used as API-reference material; this prototype does not copy its Java implementation or redistribute its engine. JDownloader and related product names are used only to identify compatibility. This independent project is not affiliated with or endorsed by JDownloader or AppWork.

Resolver Reach's third-party dependencies and notices remain applicable to the Resolver Reach binary. Each distributed application remains subject to its own license and third-party terms. A future version that incorporates or redistributes JDownloader source or binaries must complete a separate license/dependency audit and meet all applicable GPL-3.0-or-later and third-party obligations before publication.


## Architecture-specific installers

- x86-64: `JDownloaderReach-Setup-x64.exe`, limited to x64 Windows.
- ARM64: `JDownloaderReach-Setup-arm64.exe`, limited to Windows ARM64.
- Both installers include matching-architecture builds of JDownloader Reach and Resolver Reach. Each app is self-contained for its target runtime; the .NET runtime does not need to be installed separately.
- The installer workflow checks the PE machine type of both application EXEs and fails the build if the machine type does not match the selected architecture.
- CI publishes the installers as GitHub Actions artifacts with 30-day retention. These are build artifacts, not permanent GitHub Releases.
