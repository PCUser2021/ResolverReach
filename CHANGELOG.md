# Resolver Reach changelog

## [2.3.0] — 2026-10-23

### Updated
- Updated the Resolver Reach desktop stack to Avalonia 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Reactive 24.2.0, ReactiveUI.Avalonia.Reactive 12.1.6, and confirmed DynamicData 9.5.0 as current.
- Pinned the CI to the stable .NET SDK 10.0.402; .NET 10 remains the target framework rather than adopting a prerelease .NET 11 SDK.
- Updated the GitHub Actions used by Resolver Reach to checkout v7.0.1, setup-dotnet v6.0.0, upload-artifact v7.0.2, download-artifact v8.0.2, and action-gh-release v3.0.3.
- Upgraded the installer compiler to Inno Setup 7.1.0 and updated the CI's compiler path and installer version.
- Replaced all Resolver Reach SVG icon assets and the public distribution artwork with an original, blue-gradient network mark and matching white/cyan icon family inspired by the rounded geometry and blue gradient in the supplied visual reference.
- Updated the in-window icon geometry and accent palette to match the new identity.
- Kept the existing DoH validation and session logging behavior.

### Validation
- **Pending CI:** the Avalonia 12 / ReactiveUI 24 upgrade includes major-version changes. The actual x64 and ARM64 build/installer jobs must pass before the release is considered verified.
- CI build success alone does not test real endpoint availability, physical ARM64 hardware, or interactive UI behavior.


## [2.2.0] — 2026-10-23

### Added
- Added a self-contained native Windows ARM64 build and installer alongside the x64-compatible installer.
- Added architecture-specific Inno Setup packaging and two download assets: `ResolverReach-Setup-x64.exe` and `ResolverReach-Setup-arm64.exe`.
- Replaced the header `R` monogram and Unicode decoration glyphs with an original dark-titanium/glass icon family using spectral cyan, blue, violet, and magenta accents.
- Added vector status indicators for PASS, DNS ERROR, and failed/cancelled results, and redesigned scan, paste-links, DoH-check, save-log, empty-state, executable, and installer icon artwork.
- Kept the 2.1.1 scanner resilience work: 12-second connect timeout, 20-second request timeout, six concurrent probes, one retry for timeout/transient HTTP statuses, and HTTP/1.1 fallback after detected HTTP/2 protocol errors.

### Validation
- **Verified:** [Windows x64 + ARM64 build and release workflow #39945785064](https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064) completed successfully. [Release v2.2.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0) includes both `ResolverReach-Setup-x64.exe` (33,974,496 bytes) and `ResolverReach-Setup-arm64.exe` (31,276,362 bytes). CI verifies build and packaging, not endpoint availability or interactive UI behavior.
- Verified: [Windows x64 + ARM64 build and release workflow #39945785064](https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064) completed successfully. [Release v2.2.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0) includes `ResolverReach-Setup-x64.exe` (33,974,496 bytes) and `ResolverReach-Setup-arm64.exe` (31,276,362 bytes). CI verifies compilation and packaging, not live resolver reachability.
- Build and packaging success does not prove every public resolver is reachable or that the UI has been interactively exercised on both architectures.

## [2.1.1] — 2026-10-23

### Fixed
- Increased connection timeout from 5 to 12 seconds and whole-request timeout from 10 to 20 seconds to reduce false timeout failures on slow/VPN-routed connections.
- Added one bounded retry for request timeouts and transient HTTP responses (408, 425, 429, 500, 502, 503, 504).
- Added an explicit HTTP/1.1 retry if an HTTP/2 protocol reset/error occurs.
- Corrected final scan summary wording: transport failures and HTTP responses that fail DoH validation are now counted separately instead of labelling every failure as "unavailable".
- Reduced concurrent requests from 8 to 6 for large catalog scans.

### Validation
- Verified [Windows build and release workflow #39940585354](https://github.com/PCUser2021/ResolverReach/actions/runs/39940585354). Release [v2.1.1](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.1) contains `ResolverReach-Setup-x64.exe`.

## [2.1.0] — 2026-10-23

### Fixed
- Fixed title/version overlap with Windows caption buttons by reserving a safe area in the header.

### Added
- Added a visible **Save Logs** button and a native ZIP save dialog.
- ZIP entries: `session-summary.txt`, `activity.log`, `resolver-results-history.csv`, `current-visible-results.csv`, and `session.json`.

