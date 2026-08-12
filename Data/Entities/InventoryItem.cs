using LoreWeaver.Features.Characters.Catalog;

namespace LoreWeaver.Data.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public string? ItemIndex { get; set; }
    public string? ItemFreeText { get; set; }

    public int Quantity { get; set; } = 1;

    public bool IsEquipped { get; set; }
    public EquipmentSlot? Slot { get; set; }

    // Only meaningful for homebrew armor (ItemFreeText set, no catalog entry
    // to pull ArmorInfo from) - lets a homebrew piece still count toward AC.
    public ArmorCategory? ArmorCategoryOverride { get; set; }
    public int? ArmorBaseOverride { get; set; }
    public bool? ArmorDexBonusOverride { get; set; }
    public int? ArmorMaxBonusOverride { get; set; }

    public EquipmentInfo? CatalogEntry => ItemIndex is null ? null : EquipmentCatalog.Find(ItemIndex);

    public string DisplayName => CatalogEntry?.Name ?? ItemFreeText ?? "Homebrew";

    public ArmorInfo? EffectiveArmor =>
        CatalogEntry?.Armor ?? (ArmorBaseOverride is { } baseValue
            ? new ArmorInfo(ArmorCategoryOverride ?? ArmorCategory.Light, baseValue, ArmorDexBonusOverride ?? true, ArmorMaxBonusOverride)
            : null);
}
