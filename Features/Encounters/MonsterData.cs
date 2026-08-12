namespace LoreWeaver.Features.Encounters;

public record Monster(string Index, string Name, string Cr, string[] Environments, string SourcePath)
{
    public int Xp => XpTables.CrXp.GetValueOrDefault(Cr, 0);
    public string CanonicalLink => $"https://www.dnd5eapi.co{SourcePath}";
}

// Biome tags are homebrew metadata - the public SRD API has no environment data.
public static class MonsterData
{
    public static readonly IReadOnlyList<Monster> Library =
    [
        new("goblin", "Goblin", "1/4", ["Floresta", "Caverna", "Pântano"], "/api/monsters/goblin"),
        new("hobgoblin", "Hobgoblin", "1/2", ["Floresta", "Montanha", "Campos"], "/api/monsters/hobgoblin"),
        new("orc", "Orc", "1/2", ["Montanha", "Campos", "Floresta"], "/api/monsters/orc"),
        new("bandit", "Bandit", "1/8", ["Urbano", "Campos", "Costa"], "/api/monsters/bandit"),
        new("bandit-captain", "Bandit Captain", "2", ["Urbano", "Costa"], "/api/monsters/bandit-captain"),
        new("scout", "Scout", "1/2", ["Floresta", "Campos", "Costa"], "/api/monsters/scout"),
        new("veteran", "Veteran", "3", ["Urbano", "Campos"], "/api/monsters/veteran"),
        new("wolf", "Wolf", "1/4", ["Floresta", "Campos", "Ártico"], "/api/monsters/wolf"),
        new("dire-wolf", "Dire Wolf", "1", ["Floresta", "Campos", "Montanha"], "/api/monsters/dire-wolf"),
        new("giant-spider", "Giant Spider", "1", ["Floresta", "Pântano", "Caverna"], "/api/monsters/giant-spider"),
        new("giant-toad", "Giant Toad", "1", ["Pântano", "Costa"], "/api/monsters/giant-toad"),
        new("ogre", "Ogre", "2", ["Montanha", "Floresta", "Pântano"], "/api/monsters/ogre"),
        new("troll", "Troll", "5", ["Pântano", "Montanha"], "/api/monsters/troll"),
        new("skeleton", "Skeleton", "1/4", ["Ruínas", "Caverna", "Urbano"], "/api/monsters/skeleton"),
        new("zombie", "Zombie", "1/4", ["Ruínas", "Urbano", "Pântano"], "/api/monsters/zombie"),
        new("ghoul", "Ghoul", "1", ["Ruínas", "Urbano"], "/api/monsters/ghoul"),
        new("wight", "Wight", "3", ["Ruínas", "Caverna"], "/api/monsters/wight"),
        new("bulette", "Bulette", "5", ["Campos", "Montanha"], "/api/monsters/bulette"),
        new("giant-vulture", "Giant Vulture", "1", ["Deserto", "Campos"], "/api/monsters/giant-vulture"),
        new("dust-mephit", "Dust Mephit", "1/2", ["Deserto", "Planar"], "/api/monsters/dust-mephit"),
        new("quaggoth", "Quaggoth", "2", ["Subterrâneo"], "/api/monsters/quaggoth"),
        new("ettercap", "Ettercap", "2", ["Floresta", "Pântano"], "/api/monsters/ettercap"),
        new("giant-constrictor-snake", "Giant Constrictor Snake", "2", ["Floresta", "Pântano"], "/api/monsters/giant-constrictor-snake"),
        new("giant-elk", "Giant Elk", "2", ["Campos", "Floresta", "Ártico"], "/api/monsters/giant-elk"),
        new("polar-bear", "Polar Bear", "2", ["Ártico", "Costa"], "/api/monsters/polar-bear")
    ];

    public static readonly IReadOnlyDictionary<string, string[]> BiomeMap = new Dictionary<string, string[]>
    {
        ["Floresta"] = ["Floresta"],
        ["Campos"] = ["Campos"],
        ["Urbano"] = ["Urbano", "Ruínas"],
        ["Montanha"] = ["Montanha"],
        ["Deserto"] = ["Deserto"],
        ["Costa"] = ["Costa"],
        ["Pântano"] = ["Pântano"],
        ["Ártico"] = ["Ártico"],
        ["Subterrâneo"] = ["Subterrâneo", "Caverna"],
        ["Qualquer"] = []
    };
}
