using LoreWeaver.Features.Characters.Catalog;

namespace LoreWeaver.Data.Entities;

// One row per AbilityType per Character (6), seeded when the character is created.
public class CharacterSavingThrowProficiency
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public AbilityType Ability { get; set; }
    public bool IsProficient { get; set; }
    public int? BonusOverride { get; set; }
}
