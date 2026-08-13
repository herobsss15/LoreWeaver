namespace LoreWeaver.Data.Entities;

public class GameSession
{
    public Guid Id { get; set; }
    public required string RoomName { get; set; }
    public required string StartedByName { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? EndedAt { get; set; }
}
