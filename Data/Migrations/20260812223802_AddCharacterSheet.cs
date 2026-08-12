using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoreWeaver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterSheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArmorClassOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Charisma_ModifierOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Charisma_Score",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Constitution_ModifierOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Constitution_Score",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity_ModifierOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity_Score",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HitPointsCurrent",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HitPointsMaxOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Intelligence_ModifierOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Intelligence_Score",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProficiencyBonusOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaceFreeText",
                table: "Characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaceIndex",
                table: "Characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Strength_ModifierOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Strength_Score",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubraceFreeText",
                table: "Characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubraceIndex",
                table: "Characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Wisdom_ModifierOverride",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Wisdom_Score",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CharacterClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ClassIndex = table.Column<string>(type: "text", nullable: true),
                    ClassFreeText = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    IsStartingClass = table.Column<bool>(type: "boolean", nullable: false),
                    HitDieOverride = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterClasses_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSavingThrowProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Ability = table.Column<int>(type: "integer", nullable: false),
                    IsProficient = table.Column<bool>(type: "boolean", nullable: false),
                    BonusOverride = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSavingThrowProficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterSavingThrowProficiencies_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSkillProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Skill = table.Column<int>(type: "integer", nullable: false),
                    IsProficient = table.Column<bool>(type: "boolean", nullable: false),
                    BonusOverride = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkillProficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterSkillProficiencies_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClass_OneStartingClassPerCharacter",
                table: "CharacterClasses",
                column: "CharacterId",
                unique: true,
                filter: "\"IsStartingClass\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSavingThrowProficiencies_CharacterId_Ability",
                table: "CharacterSavingThrowProficiencies",
                columns: new[] { "CharacterId", "Ability" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkillProficiencies_CharacterId_Skill",
                table: "CharacterSkillProficiencies",
                columns: new[] { "CharacterId", "Skill" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClasses");

            migrationBuilder.DropTable(
                name: "CharacterSavingThrowProficiencies");

            migrationBuilder.DropTable(
                name: "CharacterSkillProficiencies");

            migrationBuilder.DropColumn(
                name: "ArmorClassOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Charisma_ModifierOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Charisma_Score",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Constitution_ModifierOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Constitution_Score",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Dexterity_ModifierOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Dexterity_Score",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HitPointsCurrent",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HitPointsMaxOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Intelligence_ModifierOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Intelligence_Score",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ProficiencyBonusOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RaceFreeText",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RaceIndex",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Strength_ModifierOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Strength_Score",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SubraceFreeText",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SubraceIndex",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Wisdom_ModifierOverride",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Wisdom_Score",
                table: "Characters");
        }
    }
}
