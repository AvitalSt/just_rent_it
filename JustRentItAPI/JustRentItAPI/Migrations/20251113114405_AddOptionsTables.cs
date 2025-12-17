using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustRentItAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DressAgeGroups_AgeGroup_AgeGroupID",
                table: "DressAgeGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_DressCities_City_CityID",
                table: "DressCities");

            migrationBuilder.DropForeignKey(
                name: "FK_DressColors_Color_ColorID",
                table: "DressColors");

            migrationBuilder.DropForeignKey(
                name: "FK_DressEventTypes_EventType_EventTypeID",
                table: "DressEventTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_DressSizes_Size_SizeID",
                table: "DressSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventType",
                table: "EventType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Color",
                table: "Color");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgeGroup",
                table: "AgeGroup");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "Sizes");

            migrationBuilder.RenameTable(
                name: "EventType",
                newName: "EventTypes");

            migrationBuilder.RenameTable(
                name: "Color",
                newName: "Colors");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.RenameTable(
                name: "AgeGroup",
                newName: "AgeGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "SizeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTypes",
                table: "EventTypes",
                column: "EventTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "ColorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "CityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgeGroups",
                table: "AgeGroups",
                column: "AgeGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_DressAgeGroups_AgeGroups_AgeGroupID",
                table: "DressAgeGroups",
                column: "AgeGroupID",
                principalTable: "AgeGroups",
                principalColumn: "AgeGroupID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressCities_Cities_CityID",
                table: "DressCities",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressColors_Colors_ColorID",
                table: "DressColors",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ColorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressEventTypes_EventTypes_EventTypeID",
                table: "DressEventTypes",
                column: "EventTypeID",
                principalTable: "EventTypes",
                principalColumn: "EventTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressSizes_Sizes_SizeID",
                table: "DressSizes",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "SizeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DressAgeGroups_AgeGroups_AgeGroupID",
                table: "DressAgeGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_DressCities_Cities_CityID",
                table: "DressCities");

            migrationBuilder.DropForeignKey(
                name: "FK_DressColors_Colors_ColorID",
                table: "DressColors");

            migrationBuilder.DropForeignKey(
                name: "FK_DressEventTypes_EventTypes_EventTypeID",
                table: "DressEventTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_DressSizes_Sizes_SizeID",
                table: "DressSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTypes",
                table: "EventTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgeGroups",
                table: "AgeGroups");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.RenameTable(
                name: "EventTypes",
                newName: "EventType");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Color");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.RenameTable(
                name: "AgeGroups",
                newName: "AgeGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "SizeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventType",
                table: "EventType",
                column: "EventTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Color",
                table: "Color",
                column: "ColorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "CityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgeGroup",
                table: "AgeGroup",
                column: "AgeGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_DressAgeGroups_AgeGroup_AgeGroupID",
                table: "DressAgeGroups",
                column: "AgeGroupID",
                principalTable: "AgeGroup",
                principalColumn: "AgeGroupID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressCities_City_CityID",
                table: "DressCities",
                column: "CityID",
                principalTable: "City",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressColors_Color_ColorID",
                table: "DressColors",
                column: "ColorID",
                principalTable: "Color",
                principalColumn: "ColorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressEventTypes_EventType_EventTypeID",
                table: "DressEventTypes",
                column: "EventTypeID",
                principalTable: "EventType",
                principalColumn: "EventTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DressSizes_Size_SizeID",
                table: "DressSizes",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "SizeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
