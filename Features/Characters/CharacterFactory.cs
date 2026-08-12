using LoreWeaver.Data.Entities;
using LoreWeaver.Features.Characters.Catalog;

namespace LoreWeaver.Features.Characters;

// A Character is never valid half-seeded: it always has all 18 skill rows
// and all 6 saving throw rows from the moment it's created.
public static class CharacterFactory
{
    public static Character Create(string name, int worldId, string? playerName = null)
    {
        var character = new Character
        {
            Name = name,
            WorldId = worldId,
            PlayerName = playerName
        };

        foreach (var skill in Enum.GetValues<Skill>())
        {
            character.Skills.Add(new CharacterSkillProficiency { Skill = skill });
        }

        foreach (var ability in Enum.GetValues<AbilityType>())
        {
            character.SavingThrows.Add(new CharacterSavingThrowProficiency { Ability = ability });
        }

        return character;
    }
}
