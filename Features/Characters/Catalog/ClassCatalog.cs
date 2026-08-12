namespace LoreWeaver.Features.Characters.Catalog;

public record ClassInfo(string Index, string Name, int HitDie, AbilityType[] SavingThrows);

// hit_die and saving_throws only - proficiency choices, equipment, spellcasting and subclasses are out of scope for now.
public static class ClassCatalog
{
    public static readonly IReadOnlyList<ClassInfo> Classes =
    [
        new("barbarian", "Barbarian", 12, [AbilityType.Strength, AbilityType.Constitution]),
        new("bard", "Bard", 8, [AbilityType.Dexterity, AbilityType.Charisma]),
        new("cleric", "Cleric", 8, [AbilityType.Wisdom, AbilityType.Charisma]),
        new("druid", "Druid", 8, [AbilityType.Intelligence, AbilityType.Wisdom]),
        new("fighter", "Fighter", 10, [AbilityType.Strength, AbilityType.Constitution]),
        new("monk", "Monk", 8, [AbilityType.Strength, AbilityType.Dexterity]),
        new("paladin", "Paladin", 10, [AbilityType.Wisdom, AbilityType.Charisma]),
        new("ranger", "Ranger", 10, [AbilityType.Strength, AbilityType.Dexterity]),
        new("rogue", "Rogue", 8, [AbilityType.Dexterity, AbilityType.Intelligence]),
        new("sorcerer", "Sorcerer", 6, [AbilityType.Constitution, AbilityType.Charisma]),
        new("warlock", "Warlock", 8, [AbilityType.Wisdom, AbilityType.Charisma]),
        new("wizard", "Wizard", 6, [AbilityType.Intelligence, AbilityType.Wisdom])
    ];

    public static ClassInfo? Find(string? index) =>
        string.IsNullOrEmpty(index) ? null : Classes.FirstOrDefault(c => c.Index == index);
}
