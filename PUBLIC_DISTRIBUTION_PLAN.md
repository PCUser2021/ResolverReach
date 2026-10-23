# Resolver Reach license, releases, and ZIP logs

Updated: 2026-10-23

## License
The original project source, documentation, and original project assets are licensed under the MIT License in root `LICENSE`. Users may use, copy, modify, distribute, sublicense, and sell copies for any purpose, subject to preserving copyright/license notice. Third-party dependencies retain their own licenses.

## Current version
Resolver Reach 2.1.0 fixes the title/version badge collision with Windows caption buttons and adds Save Logs ZIP export. Build and release verified in run #39937042568.

## Download links
- Stable direct installer URL: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Fixed version 2.1.0 URL: https://github.com/PCUser2021/ResolverReach/releases/download/v2.1.0/ResolverReach-Setup-x64.exe
- All releases: https://github.com/PCUser2021/ResolverReach/releases
- The main Windows workflow successfully published [Release v2.1.0](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.0) in [run #39937042568](https://github.com/PCUser2021/ResolverReach/actions/runs/39937042568). Asset `ResolverReach-Setup-x64.exe` (33,980,655 bytes) is uploaded. The stable latest-release URL is active for repository users.

## ZIP logs
The Save Logs button opens a native save picker with ZIP filtering and a timestamped suggested filename. The ZIP includes a session summary, complete in-session activity log, result history across all scans in this process, currently visible results, and JSON session/runtime metadata. It only captures data from the current application session.

## Visibility and screenshot
Repository visibility last verified as private, so public source and anonymous downloads require owner action in GitHub Settings. The README uses the existing design preview, not a real screenshot captured from a running Windows app.


## Resolver Reach 2.1.1 DoH reliability
- Uploaded log showed 236/266 valid DNS-over-HTTPS replies, not zero: 235 NOERROR and 1 RCODE REFUSED. The 30 fails included 11 timeouts, 11 TLS/HTTP2, 4 HTTP errors, 3 invalid DNS IDs, and 1 HTML MIME response.
- v2.1.1 raises the connect/request timeouts, retries transient timeout/status errors once, falls back to HTTP/1.1 after HTTP/2 protocol errors, uses six concurrent probes, and separates transport failures from HTTP replies that fail DoH validation.
- Build/release [run #39940585354](https://github.com/PCUser2021/ResolverReach/actions/runs/39940585354) completed successfully. Published [Release v2.1.1](https://github.com/PCUser2021/ResolverReach/releases/tag/v2.1.1), asset `ResolverReach-Setup-x64.exe` (33,979,934 bytes). A fresh scan is still necessary to confirm measured endpoint-level improvement.


## Resolver Reach 2.2.0 dual-architecture publication (2026-10-23)
- The next release builds self-contained `win-x64` and native `win-arm64` applications.
- Planned GitHub Release v2.2.0 assets: `ResolverReach-Setup-x64.exe` and `ResolverReach-Setup-arm64.exe`; the release job waits for both architecture jobs before uploading.
- Stable links after publication: `/releases/latest/download/ResolverReach-Setup-x64.exe` and `/releases/latest/download/ResolverReach-Setup-arm64.exe`.
- The icon family uses original dark-metallic spectral-glass artwork and XAML vectors, not copies of Logic Pro's exact copyrighted icon.
- New log archive retrieval failed from the supplied MediaFire URL; request a direct file upload for a new failure analysis.
- Do not claim release availability until Actions and the Releases API confirm both assets.


## Resolver Reach 2.2.0 dual architecture published
- Workflow: https://github.com/PCUser2021/ResolverReach/actions/runs/39945785064.
- Release: https://github.com/PCUser2021/ResolverReach/releases/tag/v2.2.0.
- x64 asset: ResolverReach-Setup-x64.exe (33,974,496 bytes).
- native ARM64 asset: ResolverReach-Setup-arm64.exe (31,276,362 bytes).
- The direct latest download URL is /releases/latest/download/ResolverReach-Setup-x64.exe or /releases/latest/download/ResolverReach-Setup-arm64.exe.
- CI does not prove live resolver availability. The new MediaFire ZIP could not be accessed here, so the user should attach it directly for a detailed failure analysis.


## Full repository handoff and latest diagnosis state
- Added detailed private project record `AI_SESSION_HANDOFF.md` and cross-project `Software_Project_Master_Context.md` at the repository root.
- Resolver Reach release remains v2.2.0 with verified x64 + native ARM64 setup files; CI workflow #39945785064 succeeded.
- New log `ResolverReach-Logs-20261023-040016.zip` was not attached/currently mounted, and external fetch failed. Failure-level diagnosis remains pending direct upload.
- The current GitHub repository is private, so release assets need repository access.
- Repository context is not a backup of the complete account-level ChatGPT memory database.
