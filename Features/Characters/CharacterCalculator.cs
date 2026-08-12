using LoreWeaver.Data.Entities;
using LoreWeaver.Features.Characters.Catalog;

namespace LoreWeaver.Features.Characters;

// Every derived value here: an explicit override wins, otherwise fall back to
// the standard 5e formula. Nothing is persisted - always recomputed on read.
public static class CharacterCalculator
{
    public static AbilityScoreValue Ability(this Character character, AbilityType type) => type switch
    {
        AbilityType.Strength => character.Strength,
        AbilityType.Dexterity => character.Dexterity,
        AbilityType.Constitution => character.Constitution,
        AbilityType.Intelligence => character.Intelligence,
        AbilityType.Wisdom => character.Wisdom,
        AbilityType.Charisma => character.Charisma,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    public static int ProficiencyBonus(this Character character) =>
        character.ProficiencyBonusOverride ?? (2 + (Math.Max(character.TotalLevel, 1) - 1) / 4);

    public static int ArmorClass(this Character character)
    {
        if (character.ArmorClassOverride is { } overrideValue) return overrideValue;

        var dexModifier = character.Dexterity.Modifier;
        var bodyArmor = EquippedArmor(character, EquipmentSlot.BodyArmor);

        var baseAc = bodyArmor is null
            ? 10 + dexModifier
            : bodyArmor.Base + (bodyArmor.DexBonus
                ? (bodyArmor.MaxBonus is { } maxBonus ? Math.Min(dexModifier, maxBonus) : dexModifier)
                : 0);

        // A shield is worn alongside armor (or nothing), never in place of
        // it, so its Base is always an addition, never a replacement.
        var shield = EquippedArmor(character, EquipmentSlot.Shield);
        return baseAc + (shield?.Base ?? 0);
    }

    private static ArmorInfo? EquippedArmor(Character character, EquipmentSlot slot) =>
        character.Inventory.FirstOrDefault(i => i.IsEquipped && i.Slot == slot)?.EffectiveArmor;

    public static int HitPointsMax(this Character character)
    {
        if (character.HitPointsMaxOverride is { } overrideValue) return overrideValue;

        var conModifier = character.Constitution.Modifier;
        var total = 0;

        foreach (var characterClass in character.Classes)
        {
            var hitDie = characterClass.HitDie ?? 0;
            var averagePerLevel = hitDie / 2 + 1 + conModifier;

            if (characterClass.IsStartingClass)
            {
                total += hitDie + conModifier;
                total += (characterClass.Level - 1) * averagePerLevel;
            }
            else
            {
                total += characterClass.Level * averagePerLevel;
            }
        }

        return total;
    }

    public static int SkillBonus(this Character character, CharacterSkillProficiency skill) =>
        skill.BonusOverride ?? BaseBonus(character, SkillCatalog.AbilityFor[skill.Skill], skill.IsProficient);

    public static int SaveBonus(this Character character, CharacterSavingThrowProficiency save) =>
        save.BonusOverride ?? BaseBonus(character, save.Ability, save.IsProficient);

    private static int BaseBonus(Character character, AbilityType ability, bool isProficient) =>
        character.Ability(ability).Modifier + (isProficient ? character.ProficiencyBonus() : 0);
}
