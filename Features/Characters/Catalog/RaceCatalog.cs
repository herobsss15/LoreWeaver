namespace LoreWeaver.Features.Characters.Catalog;

public record AbilityBonus(AbilityType Ability, int Bonus);

// Only Half-Elf (2014 SRD) grants a fixed bonus plus a player choice of N more abilities at +1 each.
public record AbilityBonusChoice(int Count, AbilityType[] Options);

public record RaceInfo(
    string Index,
    string Name,
    int Speed,
    string Size,
    AbilityBonus[] AbilityBonuses,
    AbilityBonusChoice? BonusChoice = null);

public record SubraceInfo(string Index, string Name, string ParentRaceIndex, AbilityBonus[] AbilityBonuses);

public static class RaceCatalog
{
    public static readonly IReadOnlyList<RaceInfo> Races =
    [
        new("dwarf", "Dwarf", 25, "Medium", [new(AbilityType.Constitution, 2)]),
        new("elf", "Elf", 30, "Medium", [new(AbilityType.Dexterity, 2)]),
        new("halfling", "Halfling", 25, "Small", [new(AbilityType.Dexterity, 2)]),
        new("human", "Human", 30, "Medium",
        [
            new(AbilityType.Strength, 1), new(AbilityType.Dexterity, 1), new(AbilityType.Constitution, 1),
            new(AbilityType.Intelligence, 1), new(AbilityType.Wisdom, 1), new(AbilityType.Charisma, 1)
        ]),
        new("dragonborn", "Dragonborn", 30, "Medium", [new(AbilityType.Strength, 2), new(AbilityType.Charisma, 1)]),
        new("gnome", "Gnome", 25, "Small", [new(AbilityType.Intelligence, 2)]),
        new("half-elf", "Half-Elf", 30, "Medium", [new(AbilityType.Charisma, 2)],
            new AbilityBonusChoice(2,
            [
                AbilityType.Strength, AbilityType.Dexterity, AbilityType.Constitution,
                AbilityType.Intelligence, AbilityType.Wisdom
            ])),
        new("half-orc", "Half-Orc", 30, "Medium", [new(AbilityType.Strength, 2), new(AbilityType.Constitution, 1)]),
        new("tiefling", "Tiefling", 30, "Medium", [new(AbilityType.Intelligence, 1), new(AbilityType.Charisma, 2)])
    ];

    public static readonly IReadOnlyList<SubraceInfo> Subraces =
    [
        new("hill-dwarf", "Hill Dwarf", "dwarf", [new(AbilityType.Wisdom, 1)]),
        new("high-elf", "High Elf", "elf", [new(AbilityType.Intelligence, 1)]),
        new("lightfoot-halfling", "Lightfoot Halfling", "halfling", [new(AbilityType.Charisma, 1)]),
        new("rock-gnome", "Rock Gnome", "gnome", [new(AbilityType.Constitution, 1)])
    ];

    public static RaceInfo? FindRace(string? index) =>
        string.IsNullOrEmpty(index) ? null : Races.FirstOrDefault(r => r.Index == index);

    public static SubraceInfo? FindSubrace(string? index) =>
        string.IsNullOrEmpty(index) ? null : Subraces.FirstOrDefault(s => s.Index == index);

    public static IEnumerable<SubraceInfo> SubracesFor(string raceIndex) =>
        Subraces.Where(s => s.ParentRaceIndex == raceIndex);
}
