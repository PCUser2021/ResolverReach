# JDownloader Reach integration architecture

## Goal

Create a new Windows desktop experience that retains the complete functionality of the existing JDownloader engine while bringing the repository's existing tools into one shell. Keep Resolver Reach and Ollama GGUF Importer source and behavior intact.

## Repository findings

- The JDownloader source is a large Java codebase. Its build configuration references separate dependency roots, including AppWorkUtils, JDClosed, browser, update-client, and server-options source. The mirror alone should not be assumed to be a complete standalone build.
- Resolver Reach is C# / .NET 10 / Avalonia with an existing single-window DoH diagnostic UI and session ZIP logging.
- Ollama GGUF Importer is C# / .NET 10 Windows Forms with its own GGUF verification/import workflow.
- The two .NET apps can be bundled together; their UI/frameworks cannot be combined by merely changing project metadata.
- JDownloader source headers use GPL-3.0-or-later. ResolverReach's original code is MIT-licensed. A combined derivative that includes JDownloader code needs a deliberate GPL compatibility and third-party license review.

## Integration strategy

### Prototype 0.1.0

- Create a separate Avalonia app under `src/JDownloaderReach/`; do not overwrite Resolver Reach or the GGUF Importer.
- Bundle all three .NET applications through the Windows CI workflow, retaining the two existing tools as independent apps and launching them from the shell.
- Delegate download work to an installed JDownloader engine. The optional API client uses only `127.0.0.1:3128`, never an arbitrary remote host.
- Add controls for API reachability, adding links to LinkGrabber, viewing a read-only package snapshot, and starting queued downloads.
- Show explicit warnings that the local API is deprecated and may be unauthenticated.
- Keep the project name and UI clearly independent; do not imply official JDownloader endorsement.

### Next phases

1. **Confirm the API contract on a real installation.** Verify the API settings and response shapes on the user's JDownloader build. Current code shows visible errors when the API is unavailable or responses cannot be parsed.
2. **Replace deprecated local API calls if appropriate.** Evaluate the authenticated MyJDownloader API or a supported local interface before extending queue mutations, event streaming, package controls, and status updates.
3. **Build a richer unified queue UI.** Add typed download/link rows, progress, pause/resume, removal, package selection, and event-driven updates only after confirming stable API signatures.
4. **Decide whether to remain a .NET control shell or begin a native port.** A full native port requires module-by-module reimplementation and feature-parity tests for crawling, plugins, hoster accounts, chunking/resume, archive extraction, updates, and the extension system. Do not claim 100% conversion from a launcher/API adapter.
5. **Audit source/binary redistribution before shipping an engine bundle.** If JDownloader code is incorporated, preserve notices, provide corresponding source/build instructions, and comply with GPL-3.0-or-later and dependency licenses. This prototype does not copy or bundle JDownloader source or binaries.

## Security boundary

- API base address is fixed to `http://127.0.0.1:3128/`.
- No credential fields are stored by this prototype.
- It does not start a listener, change firewall rules, open a port, configure system DNS, or modify JDownloader's API settings.
- The legacy API may be unauthenticated; it must not be exposed to a non-loopback interface.
- Link strings and API responses are treated as untrusted input, and requests are limited by encoded URL length.

## Current completion state

The 0.1.0 shell is an integration prototype. It is not a Java-to-C# conversion and does not replace the original JDownloader UI. See `src/JDownloaderReach/README.md` and `src/JDownloaderReach/CHANGELOG.md`.
