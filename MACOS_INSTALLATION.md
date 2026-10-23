# Resolver Reach 2.4.0 — macOS installation

## Supported Macs

- **Apple Silicon:** `ResolverReach-2.4.0-macOS-arm64.dmg` or the matching ZIP.
- **Intel:** `ResolverReach-2.4.0-macOS-x64.dmg` or the matching ZIP.
- **Minimum OS:** macOS 15 (Sequoia), following the current .NET 10 support matrix.
- The application is self-contained; users do not need to install the .NET runtime separately.

## Install from a DMG

1. Download the package that matches the Mac's processor.
2. Open the DMG and copy **Resolver Reach.app** to **Applications**.
3. On first launch, macOS may warn that the app is from an unidentified developer because the package is not yet code-signed or notarized.
4. Do not bypass Gatekeeper unless you trust the download and have independently verified its origin and checksum. Signing/notarization will be added only after Apple Developer credentials are configured.

The ZIP contains the same app bundle and can be extracted if the DMG is inconvenient.

## Release verification

The GitHub Actions workflow is configured to build both architectures, package DMG and ZIP files, verify that all six platform installers are present and non-empty, and print SHA-256 checksums. This document does not itself prove that a build succeeded. Check the linked release and its CI run before installing.

## Current limitation

The current workflow does **not** sign or notarize macOS builds. A release can therefore trigger Gatekeeper warnings. Apple Developer signing credentials and a notarization workflow are required for a frictionless public release.
