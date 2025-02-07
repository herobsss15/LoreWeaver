using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoreWeaver.Repository.Migrations
{
    /// <inheritdoc />
    public partial class bancoatt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mundos",
                columns: table => new
                {
                    MundoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeDoMundo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoMundo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mundos", x => x.MundoId);
                });

            migrationBuilder.CreateTable(
                name: "Personagens",
                columns: table => new
                {
                    PersonagemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MundoId = table.Column<int>(type: "int", nullable: false),
                    NomePersonagem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoPersonagem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PapelPersonagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personagens", x => x.PersonagemId);
                    table.ForeignKey(
                        name: "FK_Personagens_Mundos_MundoId",
                        column: x => x.MundoId,
                        principalTable: "Mundos",
                        principalColumn: "MundoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_MundoId",
                table: "Personagens",
                column: "MundoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personagens");

            migrationBuilder.DropTable(
                name: "Mundos");
        }
    }
}
