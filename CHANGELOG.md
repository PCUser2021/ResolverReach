# Resolver Reach changelog

## [2.1.1] — 2026-10-23

### Fixed
- Increased connection timeout from 5 to 12 seconds and whole-request timeout from 10 to 20 seconds to reduce false timeout failures on slow/VPN-routed connections.
- Added one bounded retry for request timeouts and transient HTTP responses (408, 425, 429, 500, 502, 503, 504).
- Added an explicit HTTP/1.1 retry if an HTTP/2 protocol reset/error occurs.
- Corrected final scan summary wording: transport failures and HTTP responses that fail DoH validation are now counted separately instead of labelling every failure as "unavailable".
- Reduced concurrent probe requests from 8 to 6 to avoid overwhelming fragile endpoints during large resolver-catalog scans.

### Validation
- Verified: [Windows build and release workflow #39940585354](https://github.com/PCUser2021/ResolverReach/actions/runs/39940585354) completed successfully. [Resolver Reach 2.1.1](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.1) is published with `ResolverReach-Setup-x64.exe` (33,979,934 bytes). CI verifies compile/package/upload, not post-fix reachability of all public resolvers.
- No program can make permanently offline, nonexistent, rate-limited, or non-compliant public resolvers return valid replies.

## [2.1.0] — 2026-10-23

### Fixed
- Fixed the main-window header/version badge collision with Windows minimize, maximize, and close controls by reserving a caption-button safe area on the header's right side.

### Added
- Added a visible **Save Logs** button to the Resolver results header.
- Save Logs opens the native file picker, allowing the user to choose where the `.zip` diagnostic archive is saved.
- ZIP contains `session-summary.txt`, `activity.log`, `resolver-results-history.csv` (results across all scans in the current app session), `current-visible-results.csv`, and `session.json`.
- Diagnostic logging records session startup/runtime, each scan's mode and target URLs, per-endpoint status/HTTP status/latency/DNS details, cancellation/failure, and scan summaries.

### Validation
- Verified successful Windows x64 build, installer compilation, and release upload in [workflow run #39937042568](https://github.com/PCUser2021/ResolverReach/actions/runs/39937042568). Published [Release v2.1.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.0) includes `ResolverReach-Setup-x64.exe` (33,980,655 bytes). The UI was compiled in CI but not interactively exercised on a Windows desktop during this task.

## [2.0.0] — 2026-10-23

### Changed
- Replaced the previous GUI with a single-window dark-slate interface inspired by the supplied reference: compact product header, bold red status strip, rounded slate cards, and restrained pink accent.
- Reduced visible controls to the two requested testing paths: **Test cURL Resolvers** and **Paste links → Test Resolvers**.
- Kept progress, endpoint status, HTTP status, latency, and DNS response details in the main window.
- Removed the visible theme selector, Paste Links menu, and Save Logs/file-picker UX to honor the explicit two-action, single-window requirement.
- Kept version display and Escape-to-cancel while a scan is active.
- Updated product, assembly, file, HTTP User-Agent, workflow, installer, and documentation version references from 1.0.0 to 2.0.0.

### Resolver checks
- The cURL action fetches the live public resolver table from the official [cURL DNS-over-HTTPS wiki](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extracts unique HTTPS endpoints from the Base URL column, and tests each listed endpoint.
- Pasted endpoints accept newline or semicolon separators.
- Tests send a real RFC 8484 DNS query and validate the DNS response; TLS connection or HTTP success alone does not count as a valid DoH response.
- Maximum 512 unique endpoints per run. Results are point-in-time measurements, not guarantees of permanent or universal reachability.

### Validation
- The actual CI build result and artifact are recorded in the repository development log after the workflow completes. This changelog alone is not proof that a binary passed validation.


## Project license and release-download updates — 2026-10-23
- Replaced the previous personal/home-use-only terms with the MIT License for original project code, documentation, and original assets.
- Added a stable latest-release installer URL to the README.
- Configured the Windows build workflow to publish the tested installer to GitHub Releases on the explicitly designated release commit.
- These are licensing/documentation/distribution changes; application behavior and executable source were not changed, so the executable remains version 2.0.0.


## Project license and release-download updates — 2026-10-23
- Replaced the previous personal/home-use-only terms with the MIT License for original project code, documentation, and original assets.
- Added a stable latest-release installer URL to the README.
- Configured the Windows build workflow to publish the tested installer to GitHub Releases on the explicitly designated release commit.
- These are licensing/documentation/distribution changes; application behavior and executable source were not changed, so the executable remains version 2.0.0.
