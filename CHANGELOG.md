# Resolver Reach changelog

## [2.1.0] — 2026-10-23

### Fixed
- Fixed the main-window header/version badge collision with Windows minimize, maximize, and close controls by reserving a caption-button safe area on the header's right side.

### Added
- Added a visible **Save Logs** button to the Resolver results header.
- Save Logs opens the native file picker, allowing the user to choose where the `.zip` diagnostic archive is saved.
- ZIP contains `session-summary.txt`, `activity.log`, `resolver-results-history.csv` (results across all scans in the current app session), `current-visible-results.csv`, and `session.json`.
- Diagnostic logging records session startup/runtime, each scan's mode and target URLs, per-endpoint status/HTTP status/latency/DNS details, cancellation/failure, and scan summaries.

### Validation
- The Windows x64 build and release job result for 2.1.0 will be recorded after the tagged release and uploaded asset are verified. Do not treat this entry as build proof.

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
