using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Migrations
{
    public partial class changeEntityTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "TB_TEAM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "TB_TEAM",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
