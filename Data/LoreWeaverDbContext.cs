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
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();

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

            character.HasMany(c => c.Inventory)
                .WithOne(i => i.Character)
                .HasForeignKey(i => i.CharacterId)
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

        // Only BodyArmor and Shield feed the ArmorClass formula, so only
        // those two slots get a database-level exclusivity guarantee - hand
        // slots (weapons) are informational only for now.
        // Note: the index name must be passed into HasIndex() itself - two
        // HasIndex() calls over the same property with only .HasDatabaseName()
        // to distinguish them silently collapse into a single index.
        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => i.CharacterId, "IX_InventoryItem_OneEquippedBodyArmorPerCharacter")
            .IsUnique()
            .HasFilter($"\"IsEquipped\" = true AND \"Slot\" = {(int)EquipmentSlot.BodyArmor}");

        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => i.CharacterId, "IX_InventoryItem_OneEquippedShieldPerCharacter")
            .IsUnique()
            .HasFilter($"\"IsEquipped\" = true AND \"Slot\" = {(int)EquipmentSlot.Shield}");
    }
}
