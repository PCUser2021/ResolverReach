namespace ResolverReach;

internal sealed record ResolverMetadata(string ProviderName, string CountryName, string CountryFlag, string FeaturesSummary)
{
    public static ResolverMetadata ForEndpoint(string endpoint)
    {
        if (!Uri.TryCreate(endpoint, UriKind.Absolute, out Uri? uri) || uri.Scheme != Uri.UriSchemeHttps)
            return Unknown;
        string host = uri.IdnHost.TrimEnd('.').ToLowerInvariant();
        if (host == "security.cloudflare-dns.com")
            return new("Cloudflare", "United States", "🇺🇸", "DNSSEC validation · malware/threat blocking · IPv4/IPv6 · HTTPS DoH");
        if (host == "family.cloudflare-dns.com")
            return new("Cloudflare", "United States", "🇺🇸", "DNSSEC validation · malware and adult-content blocking · IPv4/IPv6 · HTTPS DoH");
        if (host == "cloudflare-dns.com" || host.EndsWith(".cloudflare-dns.com", StringComparison.Ordinal) || host == "one.one.one.one")
            return new("Cloudflare", "United States", "🇺🇸", "DNSSEC validation · IPv4/IPv6 · no content filtering on standard profile · HTTPS DoH");
        if (host == "dns.google" || host.EndsWith(".dns.google", StringComparison.Ordinal))
            return new("Google Public DNS", "United States", "🇺🇸", "DNSSEC validation · IPv4/IPv6 · DNS64 on DNS64 endpoint · HTTPS DoH");
        if (host == "dns.quad9.net" || host.EndsWith(".quad9.net", StringComparison.Ordinal))
            return new("Quad9", "Switzerland", "🇨🇭", "DNSSEC validation · malware/threat blocking on the secure standard service · IPv4/IPv6 · HTTPS DoH");
        if (host == "dns.adguard-dns.com" || host.EndsWith(".adguard-dns.com", StringComparison.Ordinal))
            return new("AdGuard DNS", "Cyprus", "🇨🇾", host.StartsWith("unfiltered.", StringComparison.Ordinal)
                ? "Unfiltered profile · IPv4/IPv6 · DNSSEC not verified for this endpoint · HTTPS DoH"
                : "Ad/tracker blocking on standard profile · family filtering depends on hostname · IPv4/IPv6 · DNSSEC requires endpoint-specific verification · HTTPS DoH");
        if (host == "dns.nextdns.io" || host.EndsWith(".nextdns.io", StringComparison.Ordinal))
            return new("NextDNS", "United States", "🇺🇸", "Configurable blocklists/parental controls · policy depends on profile/configuration · HTTPS DoH");
        if (host == "doh.opendns.com" || host.EndsWith(".opendns.com", StringComparison.Ordinal) || host == "dns.umbrella.com" || host.EndsWith(".umbrella.com", StringComparison.Ordinal))
            return new("Cisco OpenDNS / Umbrella", "United States", "🇺🇸", "Phishing/malware security · filtering depends on endpoint/account profile · HTTPS DoH · DNSSEC requires profile verification");
        if (host == "doh.cleanbrowsing.org" || host.EndsWith(".cleanbrowsing.org", StringComparison.Ordinal))
            return new("CleanBrowsing", "United States", "🇺🇸", "Family/adult/security filtering depends on endpoint profile · HTTPS DoH · DNSSEC requires verification");
        if (host == "dns.mullvad.net" || host.EndsWith(".mullvad.net", StringComparison.Ordinal))
            return new("Mullvad DNS", "Sweden", "🇸🇪", "Privacy-oriented service · filtering varies by hostname/profile · HTTPS DoH · DNSSEC not inferred from URL");
        if (host == "dns0.eu" || host.EndsWith(".dns0.eu", StringComparison.Ordinal))
            return new("DNS0", "France", "🇫🇷", "Security/content protection depends on profile · IPv4/IPv6 · HTTPS DoH · DNSSEC requires verification");
        if (host == "libredns.gr" || host.EndsWith(".libredns.gr", StringComparison.Ordinal))
            return new("LibreDNS", "Greece", "🇬🇷", "Privacy-oriented public resolver · filtering/DNSSEC require endpoint-specific verification · HTTPS DoH");
        if (host == "uncensoreddns.org" || host.EndsWith(".uncensoreddns.org", StringComparison.Ordinal))
            return new("UncensoredDNS", "Denmark", "🇩🇰", "Public resolver · anycast egress may vary · filtering/DNSSEC require endpoint verification · HTTPS DoH");
        if (host == "dnsforge.de" || host.EndsWith(".dnsforge.de", StringComparison.Ordinal))
            return new("dnsforge.de", "Germany", "🇩🇪", "Privacy/filtering depends on endpoint · DNSSEC requires endpoint-specific verification · HTTPS DoH");
        return Unknown;
    }

    private static ResolverMetadata Unknown { get; } = new("Uncatalogued provider", "Unknown", "🌐",
        "Feature profile unavailable · DNSSEC unverified · filtering/policy unknown · physical resolver location cannot be inferred from URL");
}