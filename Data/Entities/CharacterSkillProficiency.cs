using LoreWeaver.Features.Characters.Catalog;

namespace LoreWeaver.Data.Entities;

// One row per Skill per Character (18), seeded when the character is created.
public class CharacterSkillProficiency
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public Skill Skill { get; set; }
    public bool IsProficient { get; set; }
    public int? BonusOverride { get; set; }
}
