using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoreWeaver.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenhaUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Mundos",
                columns: table => new
                {
                    MundoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriadorId = table.Column<int>(type: "int", nullable: false),
                    NomeDoMundo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoMundo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mundos", x => x.MundoId);
                    table.ForeignKey(
                        name: "FK_Mundos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MundoId = table.Column<int>(type: "int", nullable: false),
                    CriadorId = table.Column<int>(type: "int", nullable: false),
                    NomeEvento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoEvento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataEvento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.EventoId);
                    table.ForeignKey(
                        name: "FK_Eventos_Mundos_MundoId",
                        column: x => x.MundoId,
                        principalTable: "Mundos",
                        principalColumn: "MundoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Versoes",
                columns: table => new
                {
                    VersaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MundoId = table.Column<int>(type: "int", nullable: false),
                    CriadorId = table.Column<int>(type: "int", nullable: false),
                    NumeroVersao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescricaoMudancas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versoes", x => x.VersaoId);
                    table.ForeignKey(
                        name: "FK_Versoes_Mundos_MundoId",
                        column: x => x.MundoId,
                        principalTable: "Mundos",
                        principalColumn: "MundoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personagens",
                columns: table => new
                {
                    PersonagemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MundoId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    CriadorId = table.Column<int>(type: "int", nullable: false),
                    NomePersonagem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoPersonagem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PapelPersonagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personagens", x => x.PersonagemId);
                    table.ForeignKey(
                        name: "FK_Personagens_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "EventoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personagens_Mundos_MundoId",
                        column: x => x.MundoId,
                        principalTable: "Mundos",
                        principalColumn: "MundoId");
                    table.ForeignKey(
                        name: "FK_Personagens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_MundoId",
                table: "Eventos",
                column: "MundoId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mundos_UsuarioId",
                table: "Mundos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_EventoId",
                table: "Personagens",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_MundoId",
                table: "Personagens",
                column: "MundoId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_UsuarioId",
                table: "Personagens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Versoes_MundoId",
                table: "Versoes",
                column: "MundoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personagens");

            migrationBuilder.DropTable(
                name: "Versoes");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Mundos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
