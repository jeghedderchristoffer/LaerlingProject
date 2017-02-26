using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaerlingProject.Data.Migrations
{
    public partial class AddedLaerlingCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "LaerlingCity",
                columns: table => new
                {
                    LaerlingID = table.Column<int>(nullable: false),
                    CityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaerlingCity", x => new { x.LaerlingID, x.CityID });
                    table.ForeignKey(
                        name: "FK_LaerlingCity_Citys_CityID",
                        column: x => x.CityID,
                        principalTable: "Citys",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LaerlingCity_LaerlingProfil_LaerlingID",
                        column: x => x.LaerlingID,
                        principalTable: "LaerlingProfil",
                        principalColumn: "LaerlingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaerlingCity_CityID",
                table: "LaerlingCity",
                column: "CityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaerlingCity");

            migrationBuilder.CreateTable(
                name: "ApplicationUserCity",
                columns: table => new
                {
                    ApplicationUserID = table.Column<string>(nullable: false),
                    CityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCity", x => new { x.ApplicationUserID, x.CityID });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCity_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCity_Citys_CityID",
                        column: x => x.CityID,
                        principalTable: "Citys",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCity_CityID",
                table: "ApplicationUserCity",
                column: "CityID");
        }
    }
}
