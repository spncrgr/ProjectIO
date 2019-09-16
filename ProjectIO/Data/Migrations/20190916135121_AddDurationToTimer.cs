using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectIO.Data.Migrations
{
    public partial class AddDurationToTimer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StopTime",
                table: "Timers",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Timers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Timers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StopTime",
                table: "Timers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
