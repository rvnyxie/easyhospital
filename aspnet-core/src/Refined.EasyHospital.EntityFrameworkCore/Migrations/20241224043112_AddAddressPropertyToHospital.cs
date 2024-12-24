﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refined.EasyHospital.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressPropertyToHospital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppHospitals",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppHospitals");
        }
    }
}