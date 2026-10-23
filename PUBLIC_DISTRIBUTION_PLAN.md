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
