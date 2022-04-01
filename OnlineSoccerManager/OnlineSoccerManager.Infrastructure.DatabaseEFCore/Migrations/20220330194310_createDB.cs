using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Migrations
{
    public partial class createDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_USER",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_TEAM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TEAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_TEAM_TB_USER_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "TB_USER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TB_PLAYER",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    IsOnTransferList = table.Column<bool>(type: "bit", nullable: false),
                    AskPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PLAYER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_PLAYER_TB_TEAM_TeamId",
                        column: x => x.TeamId,
                        principalTable: "TB_TEAM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_TRANSFER",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TRANSFER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_TRANSFER_TB_PLAYER_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "TB_PLAYER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_TRANSFER_TB_TEAM_NewTeamId",
                        column: x => x.NewTeamId,
                        principalTable: "TB_TEAM",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_TRANSFER_TB_TEAM_OldTeamId",
                        column: x => x.OldTeamId,
                        principalTable: "TB_TEAM",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "TB_USER",
                columns: new[] { "Id", "CreatedDate", "Email", "Password", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("4db89c05-1b67-4771-96a2-4b607d287123"), new DateTime(2022, 3, 30, 16, 14, 37, 755, DateTimeKind.Local).AddTicks(4636), "admin@admin.com", "admin", "admin", new DateTime(2022, 3, 30, 16, 14, 37, 758, DateTimeKind.Local).AddTicks(1509), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_TB_PLAYER_TeamId",
                table: "TB_PLAYER",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TEAM_OwnerId",
                table: "TB_TEAM",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSFER_NewTeamId",
                table: "TB_TRANSFER",
                column: "NewTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSFER_OldTeamId",
                table: "TB_TRANSFER",
                column: "OldTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSFER_PlayerId",
                table: "TB_TRANSFER",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_TRANSFER");

            migrationBuilder.DropTable(
                name: "TB_PLAYER");

            migrationBuilder.DropTable(
                name: "TB_TEAM");

            migrationBuilder.DropTable(
                name: "TB_USER");
        }
    }
}
