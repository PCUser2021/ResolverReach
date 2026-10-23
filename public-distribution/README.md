<p align="center">
  <img src="assets/resolverreach-icon.svg" alt="Resolver Reach icon" width="112" />
</p>

<h1 align="center">Resolver Reach 2.1.0</h1>

<p align="center"><strong>A single-window DNS-over-HTTPS reachability tester for Windows 11 x64.</strong></p>

<p align="center">
  <a href="https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe"><strong>Download Windows x64 installer</strong></a>
  · <a href="https://github.com/PCUser2021/ResolverReach/releases">All releases</a>
</p>

Resolver Reach checks whether DoH endpoints complete a valid DNS exchange. Test the live cURL DoH catalog or paste your own HTTPS endpoint URLs. Results, status, latency, and scan progress remain in one main window.

<p align="center">
  <img src="assets/resolverreach-preview.svg" alt="Resolver Reach 2.1.0 interface design preview" width="100%" />
</p>

*This is an illustrative design preview based on the v2.0.0 layout, not a screenshot captured from a running application.*

## Features

- **Test cURL Resolvers:** fetch the live [cURL DNS-over-HTTPS wiki](https://github.com/curl/curl/wiki/DNS-over-HTTPS), extract unique HTTPS Base URLs, and test the current list.
- **Paste links:** test one or more HTTPS DoH endpoints separated by newlines or semicolons.
- **Validate DNS, not just HTTPS:** send a real RFC 8484 query for `example.com A`; check HTTP status, `application/dns-message` content type, transaction ID, response flags, echoed question, and DNS response code.
- **Integrated results:** see endpoint status, HTTP status, latency, DNS details, and progress in the main window.
- **Bounded scans:** max 512 unique endpoints per run; Escape cancels an active scan.
- **Save Logs:** exports full current-session activity and endpoint-result history, current results, and session metadata into a user-selected ZIP archive.

Results are point-in-time and cannot guarantee future availability. The selected resolver receives the test query; use services you trust.

## Download

Use the stable [latest-release installer link](https://github.com/PCUser2021/ResolverReach/releases/latest/download/ResolverReach-Setup-x64.exe). Browse [all releases](https://github.com/PCUser2021/ResolverReach/releases) for earlier versions and release assets. Public access requires the GitHub repository itself to be public.

## License

Original project source, documentation and original assets are licensed under the [MIT License](../LICENSE). You may use, copy, modify, distribute, sublicense, and sell copies for any purpose, subject to preserving the copyright and license notice. Third-party dependencies retain their own licenses.
