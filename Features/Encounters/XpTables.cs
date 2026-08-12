namespace LoreWeaver.Features.Encounters;

public static class XpTables
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<int, int>> XpThresholds =
        new Dictionary<string, IReadOnlyDictionary<int, int>>
        {
            ["facil"] = new Dictionary<int, int>
            {
                [1] = 25, [2] = 50, [3] = 75, [4] = 125, [5] = 250, [6] = 300, [7] = 350, [8] = 450, [9] = 550, [10] = 600,
                [11] = 800, [12] = 1000, [13] = 1100, [14] = 1250, [15] = 1400, [16] = 1600, [17] = 2000, [18] = 2100, [19] = 2400, [20] = 2800
            },
            ["media"] = new Dictionary<int, int>
            {
                [1] = 50, [2] = 100, [3] = 150, [4] = 250, [5] = 500, [6] = 600, [7] = 750, [8] = 900, [9] = 1100, [10] = 1200,
                [11] = 1600, [12] = 2000, [13] = 2200, [14] = 2500, [15] = 2800, [16] = 3200, [17] = 3900, [18] = 4200, [19] = 4900, [20] = 5700
            },
            ["dificil"] = new Dictionary<int, int>
            {
                [1] = 75, [2] = 150, [3] = 225, [4] = 375, [5] = 750, [6] = 900, [7] = 1100, [8] = 1400, [9] = 1600, [10] = 1900,
                [11] = 2400, [12] = 3000, [13] = 3400, [14] = 3800, [15] = 4300, [16] = 4800, [17] = 5900, [18] = 6300, [19] = 7300, [20] = 8500
            }
        };

    public static readonly IReadOnlyDictionary<string, int> CrXp = new Dictionary<string, int>
    {
        ["0"] = 10, ["1/8"] = 25, ["1/4"] = 50, ["1/2"] = 100, ["1"] = 200, ["2"] = 450, ["3"] = 700,
        ["4"] = 1100, ["5"] = 1800, ["6"] = 2300, ["7"] = 2900, ["8"] = 3900, ["9"] = 5000, ["10"] = 5900,
        ["11"] = 7200, ["12"] = 8400, ["13"] = 10000, ["14"] = 11500, ["15"] = 13000, ["16"] = 15000,
        ["17"] = 18000, ["18"] = 20000, ["19"] = 22000, ["20"] = 25000, ["21"] = 33000, ["22"] = 41000,
        ["23"] = 50000, ["24"] = 62000, ["30"] = 155000
    };

    public static List<int> ParseLevels(string value) =>
        value.Split(',')
            .Select(part => int.TryParse(part.Trim(), out var level) ? level : (int?)null)
            .Where(level => level is >= 1 and <= 20)
            .Select(level => level!.Value)
            .ToList();

    public static int BudgetFor(IReadOnlyList<int> levels, string difficulty)
    {
        if (levels.Count == 0 || !XpThresholds.TryGetValue(difficulty, out var table))
        {
            return 0;
        }

        return levels.Sum(level => table.GetValueOrDefault(level, 0));
    }
}
