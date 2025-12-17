using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustRentItAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAmountInterest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaymentTransferred",
                table: "Interests");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Interests",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PaymentAmount",
                table: "Interests",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentTransferred",
                table: "Interests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
