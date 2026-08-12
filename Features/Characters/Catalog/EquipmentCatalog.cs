using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LoreWeaver.Features.Characters.Catalog;

public record ArmorInfo(ArmorCategory Category, int Base, bool DexBonus, int? MaxBonus);

public record EquipmentInfo(string Index, string Name, EquipmentCategory Category, ArmorInfo? Armor);

// 237 items, ported from 5e-SRD-Equipment.json (5e-bits/5e-database). Trimmed
// to only what AC and inventory categorization need - no weight (out of
// scope), damage/properties, or cost. Shipped as an embedded resource rather
// than a hand-written C# array like the smaller catalogs: still zero runtime
// network dependency, just more manageable at this size.
public static class EquipmentCatalog
{
    public static readonly IReadOnlyList<EquipmentInfo> Items = Load();

    public static EquipmentInfo? Find(string? index) =>
        string.IsNullOrEmpty(index) ? null : Items.FirstOrDefault(i => i.Index == index);

    private static IReadOnlyList<EquipmentInfo> Load()
    {
        var assembly = typeof(EquipmentCatalog).Assembly;
        var resourceName = assembly.GetManifestResourceNames().Single(n => n.EndsWith("equipment.json"));
        using var stream = assembly.GetManifestResourceStream(resourceName)!;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        var dtos = JsonSerializer.Deserialize<List<EquipmentDto>>(stream, options)!;

        return dtos.Select(dto => new EquipmentInfo(
            dto.Index,
            dto.Name,
            dto.Category,
            dto.Armor is null ? null : new ArmorInfo(dto.Armor.Category, dto.Armor.Base, dto.Armor.DexBonus, dto.Armor.MaxBonus)
        )).ToList();
    }

    private class EquipmentDto
    {
        public string Index { get; set; } = "";
        public string Name { get; set; } = "";
        public EquipmentCategory Category { get; set; }
        public ArmorDto? Armor { get; set; }
    }

    private class ArmorDto
    {
        public ArmorCategory Category { get; set; }
        public int Base { get; set; }
        public bool DexBonus { get; set; }
        public int? MaxBonus { get; set; }
    }
}
