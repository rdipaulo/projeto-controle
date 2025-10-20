using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BettingControl.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServerMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciclos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalApostado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalGanhos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LucroPrejuizo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MesReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalApostado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalGanhos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LucroPrejuizo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxaAcerto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SugestoesAnalise = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Continente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Campeonato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeCasa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeVisitante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mercado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Odd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorApostado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Resultado = table.Column<int>(type: "int", nullable: false),
                    DataAposta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CicloId = table.Column<int>(type: "int", nullable: true),
                    LucroPrejuizo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                nullable: false,
                defaultValueSql: "GETDATE()");

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
