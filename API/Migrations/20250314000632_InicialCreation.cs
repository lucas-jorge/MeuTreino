using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class InicialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATEGORIAS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(maxLength: 255, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Senha = table.Column<string>(maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EXERCICIOS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Exercicio = table.Column<string>(maxLength: 100, nullable: false),
                    Serie = table.Column<int>(nullable: false),
                    Repeticoes = table.Column<int>(nullable: false),
                    Tempo = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<long>(nullable: false),
                    CategoriaId = table.Column<int>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXERCICIOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EXERCICIOS_CATEGORIAS_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CATEGORIAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EXERCICIOS_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CATEGORIAS",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 1, "Musculação" });

            migrationBuilder.InsertData(
                table: "CATEGORIAS",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 2, "Cardio" });

            migrationBuilder.InsertData(
                table: "CATEGORIAS",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 3, "Flexibilidade" });

            migrationBuilder.InsertData(
                table: "USUARIO",
                columns: new[] { "Id", "Created", "Nome", "Senha", "Status" },
                values: new object[] { 1L, new DateTime(2025, 3, 13, 21, 6, 31, 653, DateTimeKind.Local).AddTicks(7538), "Admin", "Admin@123", 1 });

            migrationBuilder.InsertData(
                table: "USUARIO",
                columns: new[] { "Id", "Created", "Nome", "Senha", "Status" },
                values: new object[] { 2L, new DateTime(2025, 3, 13, 21, 6, 31, 653, DateTimeKind.Local).AddTicks(8256), "João", "Joao@123", 1 });

            migrationBuilder.InsertData(
                table: "EXERCICIOS",
                columns: new[] { "Id", "CategoriaId", "DataCriacao", "Exercicio", "Repeticoes", "Serie", "Tempo", "UsuarioId" },
                values: new object[] { 1, 1, new DateTime(2025, 3, 13, 21, 6, 31, 656, DateTimeKind.Local).AddTicks(3461), "Supino", 12, 4, 0, 1L });

            migrationBuilder.InsertData(
                table: "EXERCICIOS",
                columns: new[] { "Id", "CategoriaId", "DataCriacao", "Exercicio", "Repeticoes", "Serie", "Tempo", "UsuarioId" },
                values: new object[] { 2, 2, new DateTime(2025, 3, 13, 21, 6, 31, 656, DateTimeKind.Local).AddTicks(4034), "Corrida", 0, 1, 30, 1L });

            migrationBuilder.InsertData(
                table: "EXERCICIOS",
                columns: new[] { "Id", "CategoriaId", "DataCriacao", "Exercicio", "Repeticoes", "Serie", "Tempo", "UsuarioId" },
                values: new object[] { 3, 3, new DateTime(2025, 3, 13, 21, 6, 31, 656, DateTimeKind.Local).AddTicks(4051), "Alongamento", 10, 2, 15, 2L });

            migrationBuilder.CreateIndex(
                name: "IX_EXERCICIOS_CategoriaId",
                table: "EXERCICIOS",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EXERCICIOS_Exercicio",
                table: "EXERCICIOS",
                column: "Exercicio");

            migrationBuilder.CreateIndex(
                name: "IX_EXERCICIOS_UsuarioId",
                table: "EXERCICIOS",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_Nome",
                table: "USUARIO",
                column: "Nome",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EXERCICIOS");

            migrationBuilder.DropTable(
                name: "CATEGORIAS");

            migrationBuilder.DropTable(
                name: "USUARIO");
        }
    }
}
