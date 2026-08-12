using LoreWeaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoreWeaver.Data;

public class LoreWeaverDbContext(DbContextOptions<LoreWeaverDbContext> options) : DbContext(options)
{
    public DbSet<World> Worlds => Set<World>();
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<CharacterClass> CharacterClasses => Set<CharacterClass>();
    public DbSet<CharacterSkillProficiency> CharacterSkillProficiencies => Set<CharacterSkillProficiency>();
    public DbSet<CharacterSavingThrowProficiency> CharacterSavingThrowProficiencies => Set<CharacterSavingThrowProficiency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<World>()
            .HasMany(w => w.Characters)
            .WithOne(c => c.World)
            .HasForeignKey(c => c.WorldId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Character>(character =>
        {
            character.OwnsOne(c => c.Strength);
            character.OwnsOne(c => c.Dexterity);
            character.OwnsOne(c => c.Constitution);
            character.OwnsOne(c => c.Intelligence);
            character.OwnsOne(c => c.Wisdom);
            character.OwnsOne(c => c.Charisma);

            character.HasMany(c => c.Classes)
                .WithOne(cc => cc.Character)
                .HasForeignKey(cc => cc.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            character.HasMany(c => c.Skills)
                .WithOne(s => s.Character)
                .HasForeignKey(s => s.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            character.HasMany(c => c.SavingThrows)
                .WithOne(s => s.Character)
                .HasForeignKey(s => s.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Partial unique index: only one CharacterClass per Character may have IsStartingClass = true.
        modelBuilder.Entity<CharacterClass>()
            .HasIndex(cc => cc.CharacterId)
            .IsUnique()
            .HasFilter("\"IsStartingClass\" = true")
            .HasDatabaseName("IX_CharacterClass_OneStartingClassPerCharacter");

        modelBuilder.Entity<CharacterSkillProficiency>()
            .HasIndex(s => new { s.CharacterId, s.Skill })
            .IsUnique();

        modelBuilder.Entity<CharacterSavingThrowProficiency>()
            .HasIndex(s => new { s.CharacterId, s.Ability })
            .IsUnique();
    }
}
