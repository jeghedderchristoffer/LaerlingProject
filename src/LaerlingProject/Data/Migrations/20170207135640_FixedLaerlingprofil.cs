using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaerlingProject.Data.Migrations
{
    public partial class FixedLaerlingprofil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Speciale",
                table: "LaerlingProfil",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Hovedforløb",
                table: "LaerlingProfil",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Firma",
                table: "LaerlingProfil",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Speciale",
                table: "LaerlingProfil",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Hovedforløb",
                table: "LaerlingProfil",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Firma",
                table: "LaerlingProfil",
                nullable: false);
        }
    }
}
