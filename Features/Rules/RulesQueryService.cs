using LoreWeaver.Features.Common;

namespace LoreWeaver.Features.Rules;

public class RulesQueryService(SrdClient client)
{
    private static readonly TimeSpan Ttl = TimeSpan.FromHours(6);
    private static readonly TimeSpan StaleWindow = TimeSpan.FromMinutes(5);

    private readonly SwrCache<SrdRuleDetail?> _detailCache = new();
    private readonly SwrCache<List<SrdRuleSummary>> _searchCache = new();

    public async Task<RulesQueryResult> SearchByTermAsync(string term, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return new RulesQueryResult(RulesQueryStatus.NotFound, null, [], false);
        }

        var cacheKey = $"rules:search:{term.Trim().ToLowerInvariant()}";
        var (found, cached, stale) = _searchCache.TryGet(cacheKey);

        List<SrdRuleSummary> combined;
        bool cacheHit;
        if (found)
        {
            combined = cached!;
            cacheHit = !stale;
        }
        else
        {
            try
            {
                var results = await Task.WhenAll(SrdClient.Domains.Select(domain => client.SearchAsync(domain, term, ct)));
                combined = results.SelectMany(r => r).ToList();
            }
            catch (HttpRequestException ex)
            {
                return new RulesQueryResult(RulesQueryStatus.Error, null, [], false, ex.Message);
            }

            _searchCache.Set(cacheKey, combined, Ttl, StaleWindow);
            cacheHit = false;
        }

        if (combined.Count == 0)
        {
            return new RulesQueryResult(RulesQueryStatus.NotFound, null, [], cacheHit);
        }

        if (combined.Count > 1)
        {
            return new RulesQueryResult(RulesQueryStatus.Ambiguous, null, combined, cacheHit);
        }

        var only = combined[0];
        var detail = await FetchDetailAsync(only.Domain, only.Index, ct);
        return detail is null
            ? new RulesQueryResult(RulesQueryStatus.NotFound, null, [], cacheHit)
            : new RulesQueryResult(RulesQueryStatus.Ok, detail, [], cacheHit);
    }

    public async Task<RulesQueryResult> SearchBySlugAsync(string slug, CancellationToken ct = default)
    {
        var normalized = NormalizeSlug(slug);
        if (normalized.Length == 0)
        {
            return new RulesQueryResult(RulesQueryStatus.NotFound, null, [], false);
        }

        foreach (var domain in SrdClient.Domains)
        {
            var detail = await FetchDetailAsync(domain, normalized, ct);
            if (detail is not null)
            {
                return new RulesQueryResult(RulesQueryStatus.Ok, detail, [], false);
            }
        }

        return new RulesQueryResult(RulesQueryStatus.NotFound, null, [], false);
    }

    private async Task<SrdRuleDetail?> FetchDetailAsync(string domain, string index, CancellationToken ct)
    {
        var cacheKey = $"rules:detail:{domain}:{index}";
        var (found, cached, stale) = _detailCache.TryGet(cacheKey);
        if (found && !stale)
        {
            return cached;
        }

        try
        {
            var detail = await client.FetchDetailAsync(domain, index, ct);
            _detailCache.Set(cacheKey, detail, Ttl, StaleWindow);
            return detail;
        }
        catch (HttpRequestException)
        {
            // Serve a stale copy rather than nothing if the upstream is down.
            return found ? cached : null;
        }
    }

    private static string NormalizeSlug(string slug)
    {
        var lowered = slug.Trim().ToLowerInvariant();
        var chars = lowered.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var collapsed = string.Join('-', new string(chars).Split('-', StringSplitOptions.RemoveEmptyEntries));
        return collapsed;
    }
}
