using LoreWeaver.Features.Characters.Catalog;

namespace LoreWeaver.Data.Entities;

public class CharacterClass
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    // Homebrew classes have no catalog entry for HitDie, so HitDieOverride
    // is required for them to count toward HitPointsMax.
    public string? ClassIndex { get; set; }
    public string? ClassFreeText { get; set; }

    public int Level { get; set; } = 1;

    // Exactly one true per character - enforced by a partial unique index in LoreWeaverDbContext.
    public bool IsStartingClass { get; set; }

    public int? HitDieOverride { get; set; }

    public int? HitDie => HitDieOverride ?? (ClassIndex is not null ? ClassCatalog.Find(ClassIndex)?.HitDie : null);

    public string DisplayName => ClassIndex is not null
        ? ClassCatalog.Find(ClassIndex)?.Name ?? ClassIndex
        : ClassFreeText ?? "Homebrew";
}
