namespace LoreWeaver.Data.Entities;

public class World
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Character> Characters { get; set; } = new List<Character>();
}
