using System.Text.Json.Serialization;

namespace LoreWeaver.Features.Rules;

// Server-side only - never call this from client-side JS/WASM.
public class SrdClient(HttpClient httpClient)
{
    public static readonly string[] Domains = ["rules", "rule-sections"];

    public async Task<IReadOnlyList<SrdRuleSummary>> SearchAsync(string domain, string term, CancellationToken ct = default)
    {
        var query = Uri.EscapeDataString(term.Trim());
        if (query.Length == 0) return [];

        var response = await httpClient.GetFromJsonAsync<SrdSearchResponseDto>($"/api/{domain}?name={query}", ct);
        return response?.Results?.Select(item => new SrdRuleSummary(item.Index, item.Name, domain, item.Url)).ToList()
               ?? [];
    }

    public async Task<SrdRuleDetail?> FetchDetailAsync(string domain, string index, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"/api/{domain}/{index}", ct);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<SrdDetailDto>(cancellationToken: ct);
        if (dto is null) return null;

        var desc = dto.Desc ?? dto.Subsections?.FirstOrDefault()?.Desc ?? [];
        return new SrdRuleDetail(dto.Index, dto.Name, domain, dto.Url, desc);
    }

    private class SrdSearchResponseDto
    {
        [JsonPropertyName("results")]
        public List<SrdItemDto>? Results { get; set; }
    }

    private class SrdItemDto
    {
        [JsonPropertyName("index")] public string Index { get; set; } = "";
        [JsonPropertyName("name")] public string Name { get; set; } = "";
        [JsonPropertyName("url")] public string Url { get; set; } = "";
    }

    private class SrdDetailDto
    {
        [JsonPropertyName("index")] public string Index { get; set; } = "";
        [JsonPropertyName("name")] public string Name { get; set; } = "";
        [JsonPropertyName("url")] public string Url { get; set; } = "";
        [JsonPropertyName("desc")] public List<string>? Desc { get; set; }
        [JsonPropertyName("subsections")] public List<SrdSubsectionDto>? Subsections { get; set; }
    }

    private class SrdSubsectionDto
    {
        [JsonPropertyName("desc")] public List<string>? Desc { get; set; }
    }
}
