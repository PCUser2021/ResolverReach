# Resolver Reach license, releases, and ZIP logs

Updated: 2026-10-23

## License
The original project source, documentation, and original project assets are licensed under the MIT License in root `LICENSE`. Users may use, copy, modify, distribute, sublicense, and sell copies for any purpose, subject to preserving copyright/license notice. Third-party dependencies retain their own licenses.

## Current version
Resolver Reach 2.1.0 fixes the title/version badge collision with Windows caption buttons and adds a Save Logs button exporting session diagnostics to a user-selected ZIP file.

## Download links
- Stable direct installer URL: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- Fixed version 2.1.0 URL: https://github.com/PCUser2021/ResolverReach/releases/download/v2.1.0/ResolverReach-Setup-x64.exe
- All releases: https://github.com/PCUser2021/ResolverReach/releases
- The main Windows workflow publishes the installer to tag `v2.1.0` on a designated main commit containing `[publish-release]`, after the Windows x64 build succeeds.

## ZIP logs
The Save Logs button opens a native save picker with ZIP filtering and a timestamped suggested filename. The ZIP includes a session summary, complete in-session activity log, result history across all scans in this process, currently visible results, and JSON session/runtime metadata. It only captures data from the current application session.

## Visibility and screenshot
Repository visibility last verified as private, so public source and anonymous downloads require owner action in GitHub Settings. The README uses the existing design preview, not a real screenshot captured from a running Windows app.
