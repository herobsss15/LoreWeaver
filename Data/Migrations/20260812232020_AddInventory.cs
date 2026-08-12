using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoreWeaver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CopperPieces",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ElectrumPieces",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoldPieces",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlatinumPieces",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SilverPieces",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ItemIndex = table.Column<string>(type: "text", nullable: true),
                    ItemFreeText = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsEquipped = table.Column<bool>(type: "boolean", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: true),
                    ArmorCategoryOverride = table.Column<int>(type: "integer", nullable: true),
                    ArmorBaseOverride = table.Column<int>(type: "integer", nullable: true),
                    ArmorDexBonusOverride = table.Column<bool>(type: "boolean", nullable: true),
                    ArmorMaxBonusOverride = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_OneEquippedBodyArmorPerCharacter",
                table: "InventoryItems",
                column: "CharacterId",
                unique: true,
                filter: "\"IsEquipped\" = true AND \"Slot\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_OneEquippedShieldPerCharacter",
                table: "InventoryItems",
                column: "CharacterId",
                unique: true,
                filter: "\"IsEquipped\" = true AND \"Slot\" = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "CopperPieces",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ElectrumPieces",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "GoldPieces",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "PlatinumPieces",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SilverPieces",
                table: "Characters");
        }
    }
}
