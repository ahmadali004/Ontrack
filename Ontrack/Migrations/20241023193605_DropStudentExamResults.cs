using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ontrack.Migrations
{
    /// <inheritdoc />
    public partial class DropStudentExamResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentExamsResult");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentExamsResult",
                columns: table => new
                {
                    StudentExamResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassID = table.Column<int>(type: "int", nullable: true),
                    ExaminationID = table.Column<int>(type: "int", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
        }
    }
}
