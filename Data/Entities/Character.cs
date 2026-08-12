namespace LoreWeaver.Data.Entities;

// Minimal placeholder - the full mechanical 5e sheet is a separate design pass.
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
}
