# Resolver Reach changelog

## [2.3.0] — 2026-10-23 (maintenance branch; pending CI verification)

### Updated
- Updated Avalonia and Avalonia.Desktop to 12.1.4.
- Updated FluentAvaloniaUI to 3.1.0.
- Switched to `ReactiveUI.Avalonia.Reactive` 12.1.6 and `ReactiveUI.Reactive` 26.0.1 to preserve the app's existing `System.Reactive` APIs across the major-version upgrade.
- Verified DynamicData 9.5.0 as the latest stable package listed on NuGet on 2026-10-23 (package page: https://www.nuget.org/packages/DynamicData/9.5.0).
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


### Complete workflow dependency audit — 2026-10-23
- Verified NuGet's current stable versions for the packages used by the main app: Avalonia/Avalonia.Desktop 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6, ReactiveUI.Reactive 26.0.1, DynamicData 9.5.0, and System.Reactive 7.0.0 (transitive minimum of ReactiveUI.Reactive).
- Verified GitHub Releases current tags: checkout v7.0.1, setup-dotnet v6.0.0, upload-artifact v7.0.2, download-artifact v8.0.2, softprops/action-gh-release v3.0.3. Workflows use their major-version tags where appropriate.
- The Ollama GGUF Importer workflow now also uses upload-artifact@v7. Neither the app release nor its binaries have been published from this maintenance branch. Use the most recent workflow run for the branch head when deciding whether CI is green.


### CI artifact-download verification — 2026-10-23
- Added `verify-packaged-artifacts` to the Windows workflow. After both architecture-specific installers are built and uploaded, CI downloads both with `actions/download-artifact@v8` and checks that the resulting EXE files exist and are non-empty; SHA-256 hashes are printed in the job log.
- The release job depends on this verification job. Release publishing remains gated by a push to main whose head commit message contains `[publish-release]`, so routine PR builds do not publish a release.
- A prior complete run passed x64/ARM64 publish and Inno Setup packaging on revision `0249107342b29067c6d0819a52efaaba61d44c62`. The artifact-action major upgrades and the new download verification job still require a final green run on the latest maintenance branch head.
