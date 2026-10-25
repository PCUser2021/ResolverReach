using ResolverReach;
using Xunit;
namespace ResolverReach.Tests;
public sealed class ResolverMetadataTests
{
    [Fact] public void CloudflareMetadataHasCountryAndDNSSEC()
    {
        var r=new ResolverResult("https://cloudflare-dns.com/dns-query","PASS",200,12.4,"NOERROR","valid");
        Assert.Equal("Cloudflare",r.ProviderName); Assert.Equal("United States",r.CountryName);
        Assert.Equal("🇺🇸",r.CountryFlag); Assert.Contains("DNSSEC",r.FeaturesSummary,StringComparison.OrdinalIgnoreCase);
    }
    [Fact] public void UnknownURLDoesNotGetFabricatedMetadata()
    {
        var r=new ResolverResult("https://resolver.example.invalid/dns-query","FAIL",null,null,"Timed out","timeout");
        Assert.Equal("Unknown",r.CountryName); Assert.Contains("unverified",r.FeaturesSummary,StringComparison.OrdinalIgnoreCase);
    }
    [Fact] public void AccessDenialDoesNotProveCensorship()
    {
        var r=new ResolverResult("https://resolver.example/dns-query","FAIL",403,18,"No DNS response","HTTP 403");
        Assert.Contains("access restriction",r.DiagnosticExplanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("censorship",r.DiagnosticExplanation,StringComparison.OrdinalIgnoreCase);
    }
    [Fact] public void TimeoutCannotDiagnoseCensorship()
    {
        var r=new ResolverResult("https://resolver.example/dns-query","FAIL",null,null,"Timed out","Request timed out");
        Assert.Contains("cannot distinguish",r.DiagnosticExplanation,StringComparison.OrdinalIgnoreCase);
    }
    [Fact] public void SortDataRanksPassAheadOfDnsErrorAndExposesLatency()
    {
        var slow=new ResolverResult("https://a.example/dns-query","PASS",200,120,"NOERROR","ok");
        var fast=new ResolverResult("https://b.example/dns-query","PASS",200,20,"NOERROR","ok");
        var dnsError=new ResolverResult("https://c.example/dns-query","DNS ERROR",200,5,"REFUSED","valid error");
        Assert.True(fast.SortLatencyMilliseconds<slow.SortLatencyMilliseconds);
        Assert.True(fast.SortGroup<dnsError.SortGroup);
    }
}