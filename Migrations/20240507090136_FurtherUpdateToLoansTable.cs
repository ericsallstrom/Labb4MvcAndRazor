using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb4MvcAndRazor.Migrations
{
    /// <inheritdoc />
    public partial class FurtherUpdateToLoansTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanStatus",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: 1,
                column: "LoanStatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: 2,
                column: "LoanStatus",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: 3,
                column: "LoanStatus",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanStatus",
                table: "Loans");
        }
    }
}
