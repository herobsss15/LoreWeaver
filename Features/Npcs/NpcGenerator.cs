using LoreWeaver.Features.Common;

namespace LoreWeaver.Features.Npcs;

public record NpcRequest(int Quantity, string Tone, string? Location, string? RoleBias, int SeedBump)
{
    public string Seed => $"{Quantity}-{Tone}-{Location}-{RoleBias}-{SeedBump}";
}

public static class NpcGenerator
{
    public static IReadOnlyList<Npc> Generate(NpcRequest request)
    {
        var quantity = Math.Clamp(request.Quantity, 1, 10);
        var rng = new SeededRandom(request.Seed);
        var toneTraits = NpcData.Tones.GetValueOrDefault(request.Tone, NpcData.Tones["neutro"]);
        var context = string.IsNullOrWhiteSpace(request.Location) ? "local indefinido" : request.Location.Trim();

        var npcs = new List<Npc>(quantity);
        for (var i = 0; i < quantity; i++)
        {
            var name = rng.Pick(NpcData.GivenNames);
            var biasRole = NpcData.RoleForBias(request.RoleBias, rng);
            var role = biasRole?.Label ?? rng.Pick(NpcData.Roles).Label;
            var traitSource = NpcData.Traits.Concat(toneTraits).ToArray();
            var trait = rng.Pick(traitSource);
            var motivation = rng.Pick(NpcData.Motivations);
            var hook = rng.Pick(NpcData.Hooks);

            npcs.Add(new Npc(name, role, trait, motivation, hook, context));
        }

        return npcs;
    }
}
