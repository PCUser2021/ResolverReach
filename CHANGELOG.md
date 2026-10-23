# Resolver Reach changelog

## [2.3.0] — 2026-10-23 (maintenance branch; pending CI verification)

### Updated
- Updated Avalonia and Avalonia.Desktop to 12.1.4.
- Updated FluentAvaloniaUI to 3.1.0.
- Switched to `ReactiveUI.Avalonia.Reactive` 12.1.6 and `ReactiveUI.Reactive` 26.0.1 to preserve the app's existing `System.Reactive` APIs across the major-version upgrade.
- Retained DynamicData 9.5.0 because the repository already uses that release and the current package evidence did not establish a newer stable version.
- Updated GitHub Actions references to checkout v7, setup-dotnet v6, upload-artifact v7, download-artifact v8, and softprops/action-gh-release v3.0.3.
- Updated the application version to 2.3.0 and adjusted release metadata.
- Updated installer discovery to prefer Inno Setup 7 while retaining a fallback for Inno Setup 6 if the package manager supplies that version.


### Verification status
- This is a maintenance branch, not a published release.
- GitHub Actions confirmed restore succeeded, but the first Avalonia 12 / ReactiveUI 26 compile attempt failed on removed title-bar and incompatible reactive-type APIs. Fixes are committed on the maintenance branch; the next x64/ARM64 CI run is pending.
- No release has been published from this branch. Do not merge or publish v2.3.0 until both Windows architecture jobs and installer packaging pass.

### Follow-up fixes from initial CI
- Removed the obsolete FluentAvalonia `TitleBarHitTestType` assignment, which no longer exists in the upgraded title-bar API.
- Switched the main view and view model to `ReactiveUI.Avalonia.Reactive` / `ReactiveUI.Reactive` and kept `System.Reactive` imports, avoiding the incompatible `RxVoid`/`ISequencer` API family.
- Updated the product-version fallback to `2.3.0`.
- Current CI run triggered by the fixes: https://github.com/PCUser2021/ResolverReach/actions/runs/39993023775 (pending at time of this note).

## [2.2.0] — 2026-10-23



### GitHub Actions patch audit — 2026-10-23
- Verified action release tags directly against the official GitHub Releases API and moved `actions/upload-artifact` to v7 and `actions/download-artifact` to v8 (latest releases v7.0.2 and v8.0.2 published 2026-10-07). The running workflow created before this commit does not validate these final action pins; a fresh CI run is required.
