using LoreWeaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoreWeaver.Data;

public class LoreWeaverDbContext(DbContextOptions<LoreWeaverDbContext> options) : DbContext(options)
{
    public DbSet<World> Worlds => Set<World>();
    public DbSet<Character> Characters => Set<Character>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<World>()
            .HasMany(w => w.Characters)
            .WithOne(c => c.World)
            .HasForeignKey(c => c.WorldId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
