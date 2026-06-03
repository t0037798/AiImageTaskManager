using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiImageTaskManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndJwtOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ImageGenerationTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ApiTestCases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageGenerationTasks_UserId",
                table: "ImageGenerationTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiTestCases_UserId",
                table: "ApiTestCases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiTestCases_Users_UserId",
                table: "ApiTestCases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageGenerationTasks_Users_UserId",
                table: "ImageGenerationTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiTestCases_Users_UserId",
                table: "ApiTestCases");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageGenerationTasks_Users_UserId",
                table: "ImageGenerationTasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ImageGenerationTasks_UserId",
                table: "ImageGenerationTasks");

            migrationBuilder.DropIndex(
                name: "IX_ApiTestCases_UserId",
                table: "ApiTestCases");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ImageGenerationTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApiTestCases");
        }
    }
}
