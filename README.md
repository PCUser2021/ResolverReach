<p align="center">
  <img src="public-distribution/assets/resolverreach-icon.svg" alt="Resolver Reach app icon" width="108" />
</p>

<h1 align="center">Resolver Reach 2.0.0</h1>

<p align="center"><strong>Windows 11 x64 · DNS-over-HTTPS reachability testing · One main window</strong></p>

> **PERSONAL / HOME USE ONLY.** "Resolver Reach" is intended solely for personal, home use. Under no circumstances is anyone permitted to use "Resolver Reach" for any purpose other than personal, home use—this includes accessing the source code or reverse engineering it.

<p align="center">
  <img src="public-distribution/assets/resolverreach-preview.svg" alt="Resolver Reach 2.0.0 interface design preview" width="100%" />
</p>

*Image note: the graphic above is an illustrative UI design preview based on the v2.0.0 layout, not a screenshot captured from a running application.*

Resolver Reach checks whether DNS-over-HTTPS (DoH) endpoints complete a valid DNS exchange. The application exposes two testing paths; progress, aggregate status, endpoint status, latency and DNS details stay in the same main window.

## What Resolver Reach does

- **Test cURL Resolvers:** fetches the live [cURL DNS-over-HTTPS resolver catalog](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extracts HTTPS endpoints from its public table's Base URL column, removes duplicates, and tests the current list.
- **Paste links → Test Resolvers:** test your own HTTPS DoH endpoints. Paste one URL per line or separate URLs with semicolons.
- **Validate actual DoH responses:** sends a real RFC 8484 DNS query for `example.com A` and checks the HTTP status, `application/dns-message` content type, DNS transaction ID, response flags, echoed question, and DNS response code. A successful TLS connection or HTTP response alone is not counted as a valid DNS exchange.
- **See results in the same window:** view PASS, DNS ERROR, or FAIL, plus HTTP status, latency, DNS response details and scan progress.
- **Bounded scanning:** at most 512 unique endpoints per run, with Escape to cancel an active scan.

A valid DNS error response is shown separately from a failed/unreachable endpoint. Availability checks are point-in-time measurements and cannot guarantee a resolver's future or universal reachability. The selected provider receives the test DNS query; only test services you trust.

## Download

- **Latest verified build:** [Resolver Reach 2.0.0 Windows x64 installer](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989)
- The successful GitHub Actions run published the `ResolverReach-Windows-x64` installer artifact. Workflow artifacts are temporary and may expire; a permanent tagged Release asset must be uploaded separately if the binary should remain downloadable.

## License and source visibility

The personal/home-use restrictions above are the requested terms. See [LICENSE.txt](LICENSE.txt) for the distribution license.

**Source visibility:** this repository is public only after its owner changes the repository's Visibility setting in GitHub. Once public, the source and Git history are visible to visitors and GitHub permits public forks; the license wording cannot technically conceal publicly committed files. The requested restriction on source access is therefore a stated use restriction, not an access-control mechanism.

## Build verification

The v2.0.0 source was merged in commit `35ff1517f7e0c0ce42fe1a8500935123d922dd30`. The Windows x64 publish, Inno Setup installer compilation and artifact upload passed in [GitHub Actions run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989). CI build success does not mean every DNS resolver was live-tested at build time.

## Technical details

- Version: **2.0.0**
- OS target: Windows 11 x64
- Stack: C# / .NET 10, Avalonia, FluentAvalonia, ReactiveUI, DynamicData
- Resolver testing: RFC 8484 DNS-over-HTTPS with POST and GET fallback where supported
- No system DNS modification, VPN, or general traffic routing

---

### Project history and development

The repository's Git history contains the source code and development records for Resolver Reach as well as the separate Ollama GGUF Importer utility. The history also includes project prompt archives and GitHub Actions workflow logs; review those records before making the repository publicly visible.
