using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ontrack.Migrations
{
    /// <inheritdoc />
    public partial class PaymentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TuitionAmount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TuitionAmount",
                table: "Payments");
        }
    }
}
