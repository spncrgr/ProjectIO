using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectIO.Data.Migrations
{
    public partial class RenameTaskTimersEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTimers_Tasks_TaskId",
                table: "TaskTimers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTimers_AspNetUsers_UserId",
                table: "TaskTimers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTimers",
                table: "TaskTimers");

            migrationBuilder.RenameTable(
                name: "TaskTimers",
                newName: "Timers");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTimers_UserId",
                table: "Timers",
                newName: "IX_Timers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTimers_TaskId",
                table: "Timers",
                newName: "IX_Timers_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timers",
                table: "Timers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timers_Tasks_TaskId",
                table: "Timers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timers_AspNetUsers_UserId",
                table: "Timers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timers_Tasks_TaskId",
                table: "Timers");

            migrationBuilder.DropForeignKey(
                name: "FK_Timers_AspNetUsers_UserId",
                table: "Timers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Timers",
                table: "Timers");

            migrationBuilder.RenameTable(
                name: "Timers",
                newName: "TaskTimers");

            migrationBuilder.RenameIndex(
                name: "IX_Timers_UserId",
                table: "TaskTimers",
                newName: "IX_TaskTimers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Timers_TaskId",
                table: "TaskTimers",
                newName: "IX_TaskTimers_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTimers",
                table: "TaskTimers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTimers_Tasks_TaskId",
                table: "TaskTimers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTimers_AspNetUsers_UserId",
                table: "TaskTimers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
