using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiImageTaskManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApiTestCases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiTestCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Method = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    HeadersJson = table.Column<string>(type: "TEXT", nullable: true),
                    BodyJson = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedStatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiTestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiTestRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiTestCaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualStatusCode = table.Column<int>(type: "INTEGER", nullable: true),
                    ActualResponseBody = table.Column<string>(type: "TEXT", maxLength: 10000, nullable: true),
                    IsPassed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DurationMs = table.Column<long>(type: "INTEGER", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiTestRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiTestRuns_ApiTestCases_ApiTestCaseId",
                        column: x => x.ApiTestCaseId,
                        principalTable: "ApiTestCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiTestRuns_ApiTestCaseId",
                table: "ApiTestRuns",
                column: "ApiTestCaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiTestRuns");

            migrationBuilder.DropTable(
                name: "ApiTestCases");
        }
    }
}
