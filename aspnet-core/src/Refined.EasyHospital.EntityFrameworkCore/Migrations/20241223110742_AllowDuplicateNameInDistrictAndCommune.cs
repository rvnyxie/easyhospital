using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refined.EasyHospital.Migrations
{
    /// <inheritdoc />
    public partial class AllowDuplicateNameInDistrictAndCommune : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppDistricts_Name",
                table: "AppDistricts");

            migrationBuilder.DropIndex(
                name: "IX_AppCommunes_Name",
                table: "AppCommunes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppDistricts_Name",
                table: "AppDistricts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCommunes_Name",
                table: "AppCommunes",
                column: "Name",
                unique: true);
        }
    }
}
