using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refined.EasyHospital.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocalities : Migration
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

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "AppCommunes");

            migrationBuilder.AlterColumn<string>(
                name: "ProvinceCode",
                table: "AppDistricts",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "DistrictCode",
                table: "AppCommunes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "AppCommunes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProvinceCode",
                table: "AppDistricts",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "DistrictId",
                table: "AppCommunes",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

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
