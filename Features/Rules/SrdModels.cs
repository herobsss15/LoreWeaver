namespace LoreWeaver.Features.Rules;

public record SrdRuleSummary(string Index, string Name, string Domain, string Url);

public record SrdRuleDetail(string Index, string Name, string Domain, string Url, IReadOnlyList<string> Desc)
{
    public string CanonicalLink => $"https://www.dnd5eapi.co{Url}";
    public string Excerpt => Desc.Count > 0 ? Desc[0] : string.Empty;
}

public enum RulesQueryStatus
{
    Ok,
    NotFound,
    Ambiguous,
    Error
}

public record RulesQueryResult(
    RulesQueryStatus Status,
    SrdRuleDetail? Selected,
    IReadOnlyList<SrdRuleSummary> Candidates,
    bool Cached,
    string? ErrorMessage = null);
