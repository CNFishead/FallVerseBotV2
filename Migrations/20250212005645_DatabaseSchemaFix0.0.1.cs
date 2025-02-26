using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FallVerseBotV2.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseSchemaFix001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEconomy",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyAmount = table.Column<int>(type: "integer", nullable: false),
                    LastClaimed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StreakCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEconomy", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserEconomy_UserRecord_UserId",
                        column: x => x.UserId,
                        principalTable: "UserRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEconomy");

            migrationBuilder.DropTable(
                name: "UserRecord");
        }
    }
}
