namespace LoreWeaver.Data.Entities;

public class Character
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? PlayerName { get; set; }
    public string? Notes { get; set; }
    public bool Active { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public int WorldId { get; set; }
    public World? World { get; set; }

    public AbilityScoreValue Strength { get; set; } = new();
    public AbilityScoreValue Dexterity { get; set; } = new();
    public AbilityScoreValue Constitution { get; set; } = new();
    public AbilityScoreValue Intelligence { get; set; } = new();
    public AbilityScoreValue Wisdom { get; set; } = new();
    public AbilityScoreValue Charisma { get; set; } = new();

    // Race ability bonuses are added into the scores above at pick time in the
    // UI, not stored separately - a later manual score edit never loses them.
    public string? RaceIndex { get; set; }
    public string? RaceFreeText { get; set; }
    public string? SubraceIndex { get; set; }
    public string? SubraceFreeText { get; set; }

    public int HitPointsCurrent { get; set; }
    public int? HitPointsMaxOverride { get; set; }

    public int? ArmorClassOverride { get; set; }
    public int? ProficiencyBonusOverride { get; set; }

    public int CopperPieces { get; set; }
    public int SilverPieces { get; set; }
    public int ElectrumPieces { get; set; }
    public int GoldPieces { get; set; }
    public int PlatinumPieces { get; set; }

    public ICollection<CharacterClass> Classes { get; set; } = new List<CharacterClass>();
    public ICollection<CharacterSkillProficiency> Skills { get; set; } = new List<CharacterSkillProficiency>();
    public ICollection<CharacterSavingThrowProficiency> SavingThrows { get; set; } = new List<CharacterSavingThrowProficiency>();
    public ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();

    public int TotalLevel => Classes.Sum(c => c.Level);
}
