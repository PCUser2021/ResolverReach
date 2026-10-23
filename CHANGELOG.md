# Resolver Reach changelog

## Repository cleanup — 2026-10-23
- Removed the standalone GGUF importer source, icon, README, project file, and dedicated GitHub Actions workflow from the current `main` tree.
- Updated the active project context and handoff documents to describe Resolver Reach as the only current application.
- Retained Resolver Reach's source, installer, and Windows build workflow. No Resolver Reach product version was changed by this deletion-only cleanup.
- This does not rewrite old Git commits or erase historical GitHub Actions records; the pre-cleanup snapshot remains on its backup branch.

## [2.3.0] — 2026-10-23 (merged into main; CI verified; release not published)

### Updated
- Upgraded Avalonia and Avalonia.Desktop to 12.1.4 and FluentAvaloniaUI to 3.1.0.
- Switched to the System.Reactive-compatible packages `ReactiveUI.Avalonia.Reactive` 12.1.6 and `ReactiveUI.Reactive` 26.0.1.
- Verified DynamicData 9.5.0 as the latest stable NuGet package listed on 2026-10-23.
- Updated GitHub Actions to checkout v7, setup-dotnet v6, upload-artifact v7, download-artifact v8, and softprops/action-gh-release v3.0.3.
- Kept .NET 10 and updated application, assembly, file, and installer metadata to version 2.3.0.
- Migrated the main window to FluentAvalonia 3's `FAAppWindow`; removed the obsolete `TitleBarHitTestType` assignment.
- Switched the app to the `ReactiveUI.Avalonia.Reactive` / `ReactiveUI.Reactive` compatibility packages to retain existing `System.Reactive` subject, scheduler, and command types.
- Improved Inno Setup discovery for versioned Inno Setup 7/6 paths and PATH fallback.
- Added a CI artifact round-trip job to download both installers with download-artifact v8 and verify each executable is non-empty.

### Verified
- Merged PR #2, "Upgrade Resolver Reach dependencies and Windows CI toolchain": https://github.com/PCUser2021/ResolverReach/pull/2
- Squash merge commit on main: `e96baa4d822ce58617b8544e6167df94334ffea3`.
- Windows x64 and native ARM64 build, publish, Inno Setup installer compilation, upload, artifact download, and executable checks all passed on final PR head: https://github.com/PCUser2021/ResolverReach/actions/runs/39994207960
- The routine CI run skipped the release-only job as intended. The latest published release remains v2.2.0; v2.3.0 has **not** been published as a GitHub Release.
- CI validates compilation and packaging; it does not replace live network testing of public resolvers or an interactive GUI smoke test.

## [2.2.0] — 2026-10-23

### GitHub Actions patch audit — 2026-10-23
- Verified action release tags directly against the official GitHub Releases API and moved `actions/upload-artifact` to v7 and `actions/download-artifact` to v8 (latest releases v7.0.2 and v8.0.2 published 2026-10-07). The running workflow created before this commit does not validate these final action pins; a fresh CI run is required.

### Complete workflow dependency audit — 2026-10-23
- Verified NuGet's current stable versions for the packages used by the main app: Avalonia/Avalonia.Desktop 12.1.4, FluentAvaloniaUI 3.1.0, ReactiveUI.Avalonia.Reactive 12.1.6, ReactiveUI.Reactive 26.0.1, DynamicData 9.5.0, and System.Reactive 7.0.0 (transitive minimum of ReactiveUI.Reactive).
- Verified GitHub Releases current tags: checkout v7.0.1, setup-dotnet v6.0.0, upload-artifact v7.0.2, download-artifact v8.0.2, softprops/action-gh-release v3.0.3. Workflows use their major-version tags where appropriate.

### CI artifact-download verification — 2026-10-23
- Added `verify-packaged-artifacts` to the Windows workflow. After both architecture-specific installers are built and uploaded, CI downloads both with `actions/download-artifact@v8` and checks that the resulting EXE files exist and are non-empty; SHA-256 hashes are printed in the job log.
- The release job depends on this verification job. Release publishing remains gated by a push to main whose head commit message contains `[publish-release]`, so routine PR builds do not publish a release.
- A prior complete run passed x64/ARM64 publish and Inno Setup packaging on revision `0249107342b29067c6d0819a52efaaba61d44c62`. The artifact-action major upgrades and the new download verification job still require a final green run on the latest maintenance branch head.
