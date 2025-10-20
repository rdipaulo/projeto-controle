using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BettingControl.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationWithAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciclos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TotalApostado = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalGanhos = table.Column<decimal>(type: "TEXT", nullable: false),
                    LucroPrejuizo = table.Column<decimal>(type: "TEXT", nullable: false),
                    ROI = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsClosed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciclos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciclos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FechamentosMensais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MesReferencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalApostado = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalGanhos = table.Column<decimal>(type: "TEXT", nullable: false),
                    LucroPrejuizo = table.Column<decimal>(type: "TEXT", nullable: false),
                    ROI = table.Column<decimal>(type: "TEXT", nullable: false),
                    Yield = table.Column<decimal>(type: "TEXT", nullable: false),
                    TaxaAcerto = table.Column<decimal>(type: "TEXT", nullable: false),
                    SugestoesAnalise = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FechamentosMensais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FechamentosMensais_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoBancas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Saldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoBancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoBancas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pais = table.Column<string>(type: "TEXT", nullable: false),
                    Continente = table.Column<string>(type: "TEXT", nullable: false),
                    Campeonato = table.Column<string>(type: "TEXT", nullable: false),
                    TimeCasa = table.Column<string>(type: "TEXT", nullable: false),
                    TimeVisitante = table.Column<string>(type: "TEXT", nullable: false),
                    Mercado = table.Column<string>(type: "TEXT", nullable: false),
                    Odd = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValorApostado = table.Column<decimal>(type: "TEXT", nullable: false),
                    Resultado = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAposta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CicloId = table.Column<int>(type: "INTEGER", nullable: true),
                    LucroPrejuizo = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_Ciclos_CicloId",
                        column: x => x.CicloId,
                        principalTable: "Ciclos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_CicloId",
                table: "Bets",
                column: "CicloId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciclos_UserId",
                table: "Ciclos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FechamentosMensais_UserId",
                table: "FechamentosMensais",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoBancas_UserId",
                table: "HistoricoBancas",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "FechamentosMensais");

            migrationBuilder.DropTable(
                name: "HistoricoBancas");

            migrationBuilder.DropTable(
                name: "Ciclos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
