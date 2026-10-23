# Resolver Reach changelog

## [2.3.0] — 2026-10-23 (maintenance branch; pending CI verification)

### Updated
- Updated Avalonia and Avalonia.Desktop to 12.1.4.
- Updated FluentAvaloniaUI to 3.1.0.
- Updated ReactiveUI.Avalonia to 12.1.6 and ReactiveUI to 24.2.0.
- Retained DynamicData 9.5.0 because the repository already uses that release and the current package evidence did not establish a newer stable version.
- Updated GitHub Actions references to checkout v7, setup-dotnet v6, upload-artifact v6, download-artifact v6, and softprops/action-gh-release v3.0.3.
- Updated the application version to 2.3.0 and adjusted release metadata.
- Updated installer discovery to prefer Inno Setup 7 while retaining a fallback for Inno Setup 6 if the package manager supplies that version.

### Verification status
- This is a maintenance branch, not a published release.
- The repository has not been built locally in this session. The GitHub Actions CI result must be checked before merging or publishing.
- Avalonia 12 / FluentAvalonia 3 are major-version upgrades; source/API incompatibilities, if any, must be fixed from CI output before release.

## [2.2.0] — 2026-10-23

