using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustRentItAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDressTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeGroup",
                table: "Dresses");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Dresses");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Dresses");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Dresses");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Dresses");

            migrationBuilder.CreateTable(
                name: "AgeGroup",
                columns: table => new
                {
                    AgeGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameHebrew = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeGroup", x => x.AgeGroupID);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameHebrew = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ColorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameHebrew = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ColorID);
                });

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    EventTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameHebrew = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.EventTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameHebrew = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeID);
                });

            migrationBuilder.CreateTable(
                name: "DressAgeGroups",
                columns: table => new
                {
                    DressID = table.Column<int>(type: "int", nullable: false),
                    AgeGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressAgeGroups", x => new { x.DressID, x.AgeGroupID });
                    table.ForeignKey(
                        name: "FK_DressAgeGroups_AgeGroup_AgeGroupID",
                        column: x => x.AgeGroupID,
                        principalTable: "AgeGroup",
                        principalColumn: "AgeGroupID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DressAgeGroups_Dresses_DressID",
                        column: x => x.DressID,
                        principalTable: "Dresses",
                        principalColumn: "DressID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressCities",
                columns: table => new
                {
                    DressID = table.Column<int>(type: "int", nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressCities", x => new { x.DressID, x.CityID });
                    table.ForeignKey(
                        name: "FK_DressCities_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DressCities_Dresses_DressID",
                        column: x => x.DressID,
                        principalTable: "Dresses",
                        principalColumn: "DressID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressColors",
                columns: table => new
                {
                    DressID = table.Column<int>(type: "int", nullable: false),
                    ColorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressColors", x => new { x.DressID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_DressColors_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ColorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DressColors_Dresses_DressID",
                        column: x => x.DressID,
                        principalTable: "Dresses",
                        principalColumn: "DressID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressEventTypes",
                columns: table => new
                {
                    DressID = table.Column<int>(type: "int", nullable: false),
                    EventTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressEventTypes", x => new { x.DressID, x.EventTypeID });
                    table.ForeignKey(
                        name: "FK_DressEventTypes_Dresses_DressID",
                        column: x => x.DressID,
                        principalTable: "Dresses",
                        principalColumn: "DressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DressEventTypes_EventType_EventTypeID",
                        column: x => x.EventTypeID,
                        principalTable: "EventType",
                        principalColumn: "EventTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DressSizes",
                columns: table => new
                {
                    DressID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressSizes", x => new { x.DressID, x.SizeID });
                    table.ForeignKey(
                        name: "FK_DressSizes_Dresses_DressID",
                        column: x => x.DressID,
                        principalTable: "Dresses",
                        principalColumn: "DressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DressSizes_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "SizeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DressAgeGroups_AgeGroupID",
                table: "DressAgeGroups",
                column: "AgeGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_DressCities_CityID",
                table: "DressCities",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_DressColors_ColorID",
                table: "DressColors",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_DressEventTypes_EventTypeID",
                table: "DressEventTypes",
                column: "EventTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_DressSizes_SizeID",
                table: "DressSizes",
                column: "SizeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DressAgeGroups");

            migrationBuilder.DropTable(
                name: "DressCities");

            migrationBuilder.DropTable(
                name: "DressColors");

            migrationBuilder.DropTable(
                name: "DressEventTypes");

            migrationBuilder.DropTable(
                name: "DressSizes");

            migrationBuilder.DropTable(
                name: "AgeGroup");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.AddColumn<long>(
                name: "AgeGroup",
                table: "Dresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "City",
                table: "Dresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Color",
                table: "Dresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EventType",
                table: "Dresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Dresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
