using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refined.EasyHospital.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintsToProvince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppProvinces_Code",
                table: "AppProvinces",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProvinces_Name",
                table: "AppProvinces",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppProvinces_Code",
                table: "AppProvinces");

            migrationBuilder.DropIndex(
                name: "IX_AppProvinces_Name",
                table: "AppProvinces");
        }
    }
}
