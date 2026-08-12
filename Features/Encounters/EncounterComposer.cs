namespace LoreWeaver.Features.Encounters;

public record MonsterPick(Monster Monster, int Count);

public record EncounterResult(int Budget, IReadOnlyList<MonsterPick> Monsters, string? Note);

public static class EncounterComposer
{
    private const int MaxPicks = 6;
    private const int GuardLimit = 50;

    public static EncounterResult Compose(IReadOnlyList<int> levels, string difficulty, string biome)
    {
        var budget = XpTables.BudgetFor(levels, difficulty);
        if (budget == 0)
        {
            return new EncounterResult(budget, [], "Informe níveis válidos do grupo para estimar o orçamento de XP.");
        }

        var pool = FilterByBiome(biome);
        if (pool.Count == 0)
        {
            return new EncounterResult(
                budget,
                [new MonsterPick(MonsterData.Library[0], 1)],
                "Sem criaturas SRD vinculadas a este bioma. Sugestão genérica apresentada.");
        }

        var sorted = pool.OrderByDescending(m => m.Xp).ToList();
        var picks = new List<MonsterPick>();
        var remaining = budget;
        var guard = 0;

        while (remaining > 0 && picks.Count < MaxPicks && guard < GuardLimit)
        {
            guard++;
            var candidate = sorted.FirstOrDefault(m => m.Xp <= remaining) ?? sorted[^1];

            var existingIndex = picks.FindIndex(p => p.Monster.Index == candidate.Index);
            if (existingIndex >= 0)
            {
                picks[existingIndex] = picks[existingIndex] with { Count = picks[existingIndex].Count + 1 };
            }
            else
            {
                picks.Add(new MonsterPick(candidate, 1));
            }

            remaining -= candidate.Xp;

            var minXp = pool.Min(m => m.Xp);
            if (candidate.Xp > remaining && remaining > 0 && remaining < minXp)
            {
                break;
            }
        }

        var approximate = remaining > 0;
        return new EncounterResult(
            budget,
            picks.Count > 0 ? picks : [new MonsterPick(MonsterData.Library[0], 1)],
            approximate ? "Combinação aproximada; ajuste o encontro conforme necessário." : null);
    }

    private static List<Monster> FilterByBiome(string biome)
    {
        var tags = MonsterData.BiomeMap.GetValueOrDefault(biome, []);
        if (tags.Length == 0)
        {
            return MonsterData.Library.ToList();
        }

        return MonsterData.Library.Where(m => m.Environments.Any(tags.Contains)).ToList();
    }
}
