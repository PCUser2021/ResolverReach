<p align="center">
  <img src="assets/resolverreach-icon.svg" alt="Resolver Reach icon" width="112" />
</p>

<h1 align="center">Resolver Reach 2.0.0</h1>

<p align="center"><strong>A focused, single-window DNS-over-HTTPS resolver reachability tester for Windows 11 x64.</strong></p>

> **PERSONAL / HOME USE ONLY.** Resolver Reach is intended solely for personal, home use. Under no circumstances is anyone permitted to use Resolver Reach for any purpose other than personal, home use—this includes accessing the source code or reverse engineering it.

Resolver Reach checks whether DNS-over-HTTPS endpoints complete a valid DNS exchange. Choose one of two actions: test the live resolver URLs listed by the cURL DoH wiki, or paste your own HTTPS endpoint URLs. Results, status, latency, and scan progress remain in the main window.

<p align="center">
  <img src="assets/resolverreach-preview.svg" alt="Resolver Reach 2.0.0 main-window design preview" width="100%" />
</p>

*Preview note: this is a design illustration rendered from the v2.0.0 layout, not a screenshot captured from a running application.*

## Download

The release-only repository should host the installer as a GitHub Release asset named **Resolver-Reach-2.0.0-Setup-x64.exe**. The package does not include source code.

## Features

- **Test cURL Resolvers:** fetches the live [cURL DNS-over-HTTPS wiki](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extracts HTTPS endpoints from its public resolver table's Base URL column, deduplicates them, and tests the current catalog.
- **Paste links:** tests one or more HTTPS DoH endpoint URLs separated by newlines or semicolons.
- **Validate DNS, not just HTTPS:** sends a real RFC 8484 query for `example.com A`; checks HTTP status, `application/dns-message` content type, transaction ID, response flags, the echoed question, and DNS response code.
- **Integrated results:** reports endpoint status, HTTP status, latency, DNS response details, and scan progress in the same main window.
- **Bounded scan:** at most 512 unique endpoints per run; Escape cancels an active scan.

A valid HTTP response or TLS connection alone is not counted as a successful DoH exchange. Results are point-in-time measurements and cannot guarantee universal or future reachability. Your selected DNS provider receives the test query; use providers you trust.

## Personal / home-use terms

This is proprietary closed-source software. No source code is distributed or licensed, and no rights to modify, redistribute, or use the software outside personal, home use are granted. See [LICENSE.txt](LICENSE.txt).

The license states the requested restrictions, subject to any non-waivable rights under applicable law. Contractual restrictions on reverse engineering can vary by jurisdiction.

## Build verification

The Windows x64 publish and installer compilation passed in [GitHub Actions run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989). CI verifies packaging success; it does not test every public resolver's live availability.

## Product details

- Version: **2.0.0**
- Target: Windows 11 x64
- Project/source code: maintained privately and not included in this distribution
