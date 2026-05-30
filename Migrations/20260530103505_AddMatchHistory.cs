using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveGamingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Player1Email = table.Column<string>(type: "TEXT", nullable: false),
                    Player2Email = table.Column<string>(type: "TEXT", nullable: false),
                    WinnerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    EntryFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrizeMoney = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchRecords");
        }
    }
}
