namespace LoreWeaver.Features.Characters.Catalog;

// Skill-to-ability mapping is fixed by the rules, so it's embedded here rather than fetched at runtime.
public static class SkillCatalog
{
    public static readonly IReadOnlyDictionary<Skill, AbilityType> AbilityFor = new Dictionary<Skill, AbilityType>
    {
        [Skill.Acrobatics] = AbilityType.Dexterity,
        [Skill.AnimalHandling] = AbilityType.Wisdom,
        [Skill.Arcana] = AbilityType.Intelligence,
        [Skill.Athletics] = AbilityType.Strength,
        [Skill.Deception] = AbilityType.Charisma,
        [Skill.History] = AbilityType.Intelligence,
        [Skill.Insight] = AbilityType.Wisdom,
        [Skill.Intimidation] = AbilityType.Charisma,
        [Skill.Investigation] = AbilityType.Intelligence,
        [Skill.Medicine] = AbilityType.Wisdom,
        [Skill.Nature] = AbilityType.Intelligence,
        [Skill.Perception] = AbilityType.Wisdom,
        [Skill.Performance] = AbilityType.Charisma,
        [Skill.Persuasion] = AbilityType.Charisma,
        [Skill.Religion] = AbilityType.Intelligence,
        [Skill.SleightOfHand] = AbilityType.Dexterity,
        [Skill.Stealth] = AbilityType.Dexterity,
        [Skill.Survival] = AbilityType.Wisdom
    };

    public static readonly IReadOnlyDictionary<Skill, string> DisplayName = new Dictionary<Skill, string>
    {
        [Skill.Acrobatics] = "Acrobatics",
        [Skill.AnimalHandling] = "Animal Handling",
        [Skill.Arcana] = "Arcana",
        [Skill.Athletics] = "Athletics",
        [Skill.Deception] = "Deception",
        [Skill.History] = "History",
        [Skill.Insight] = "Insight",
        [Skill.Intimidation] = "Intimidation",
        [Skill.Investigation] = "Investigation",
        [Skill.Medicine] = "Medicine",
        [Skill.Nature] = "Nature",
        [Skill.Perception] = "Perception",
        [Skill.Performance] = "Performance",
        [Skill.Persuasion] = "Persuasion",
        [Skill.Religion] = "Religion",
        [Skill.SleightOfHand] = "Sleight of Hand",
        [Skill.Stealth] = "Stealth",
        [Skill.Survival] = "Survival"
    };
}
