using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustRentItAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSizeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEnglish",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "NameHebrew",
                table: "Sizes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Sizes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Sizes");

            migrationBuilder.AddColumn<string>(
                name: "NameEnglish",
                table: "Sizes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameHebrew",
                table: "Sizes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
