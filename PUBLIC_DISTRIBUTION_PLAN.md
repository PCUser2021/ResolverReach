# Resolver Reach licensing and release publication status

Updated: 2026-10-23

## User intent
The user has explicitly requested that anyone may use Resolver Reach and the project source for any purpose and that working downloads be available from GitHub Releases.

## License
- The project uses the MIT License in root `LICENSE` and compatibility copy `LICENSE.txt`, as well as `public-distribution/LICENSE.txt`.
- MIT permits any-purpose use, modification, redistribution, sublicensing, and sale of copies, provided the copyright and license notice is retained.
- This applies to original project code, documentation, and original assets authored for this repository. Third-party libraries, dependencies, and trademarks remain subject to their own terms.
- The previous personal/home-use-only restriction has been removed from the project licenses and readmes.

## Download links
- Stable direct installer URL: https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe
- All releases: https://github.com/PCUser2021/ResolverReach/releases
- `.github/workflows/windows-build.yml` is being updated to publish tag `v2.0.0` and upload `ResolverReach-Setup-x64.exe` from the verified Windows build artifact when a designated main-branch commit contains `[publish-release]`.
- Before calling the download link active, check the workflow run and confirm the Release API returns a published `v2.0.0` release with a `ResolverReach-Setup-x64.exe` browser download URL.

## Visibility
Last verified repository visibility is `private`. A permissive license does not make private GitHub source public. Repository owner must open https://github.com/PCUser2021/ResolverReach/settings, go to Danger Zone → Change repository visibility → Public, and confirm. The connected GitHub actions in this session do not expose the repository visibility mutation; do not claim that the repository is public until metadata confirms it.

## Images
- `public-distribution/assets/resolverreach-icon.svg` is the app's custom icon artwork.
- `public-distribution/assets/resolverreach-preview.svg` is an illustrative layout preview, not a screenshot captured from a running Windows app.

## Backup
- Project source, prompts and outcome records are maintained in private Git history, `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, and `DEVELOPMENT_LOG.md`.
- A full export of ChatGPT's platform-level Memory and all historical chats is not available through the connected repository tool.
