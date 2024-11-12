using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ontrack.Migrations
{
    /// <inheritdoc />
    public partial class _1029 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Teachers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OntrackUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OntrackUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentExamsResult",
                columns: table => new
                {
                    StudentExamResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    ExaminationID = table.Column<int>(type: "int", nullable: true),
                    ClassID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExamsResult", x => x.StudentExamResultID);
                    table.ForeignKey(
                        name: "FK_StudentExamsResult_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_StudentExamsResult_Examinations_ExaminationID",
                        column: x => x.ExaminationID,
                        principalTable: "Examinations",
                        principalColumn: "ExaminationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentExamsResult_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamsResult_ClassID",
                table: "StudentExamsResult",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamsResult_ExaminationID",
                table: "StudentExamsResult",
                column: "ExaminationID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamsResult_StudentID",
                table: "StudentExamsResult",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_OntrackUser_UserId",
                table: "Teachers",
                column: "UserId",
                principalTable: "OntrackUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_OntrackUser_UserId",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "OntrackUser");

            migrationBuilder.DropTable(
                name: "StudentExamsResult");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Teachers");
        }
    }
}
