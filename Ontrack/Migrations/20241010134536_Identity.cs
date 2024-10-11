using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ontrack.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Examinations_ExaminationID",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Students_StudentID",
                table: "ExamResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamResults",
                table: "ExamResults");

            migrationBuilder.RenameTable(
                name: "ExamResults",
                newName: "StudentExamsResult");

            migrationBuilder.RenameIndex(
                name: "IX_ExamResults_StudentID",
                table: "StudentExamsResult",
                newName: "IX_StudentExamsResult_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_ExamResults_ExaminationID",
                table: "StudentExamsResult",
                newName: "IX_StudentExamsResult_ExaminationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExamsResult",
                table: "StudentExamsResult",
                column: "StudentExamResultID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExamsResult_Examinations_ExaminationID",
                table: "StudentExamsResult",
                column: "ExaminationID",
                principalTable: "Examinations",
                principalColumn: "ExaminationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExamsResult_Students_StudentID",
                table: "StudentExamsResult",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentExamsResult_Examinations_ExaminationID",
                table: "StudentExamsResult");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExamsResult_Students_StudentID",
                table: "StudentExamsResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExamsResult",
                table: "StudentExamsResult");

            migrationBuilder.RenameTable(
                name: "StudentExamsResult",
                newName: "ExamResults");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExamsResult_StudentID",
                table: "ExamResults",
                newName: "IX_ExamResults_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExamsResult_ExaminationID",
                table: "ExamResults",
                newName: "IX_ExamResults_ExaminationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamResults",
                table: "ExamResults",
                column: "StudentExamResultID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Examinations_ExaminationID",
                table: "ExamResults",
                column: "ExaminationID",
                principalTable: "Examinations",
                principalColumn: "ExaminationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Students_StudentID",
                table: "ExamResults",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
