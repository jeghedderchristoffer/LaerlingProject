using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LaerlingProject.Data.Migrations
{
    public partial class AddedLaerlingProfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaerlingProfilID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fag",
                columns: table => new
                {
                    FagID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fag", x => x.FagID);
                });

            migrationBuilder.CreateTable(
                name: "LaerlingProfil",
                columns: table => new
                {
                    LaerlingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    FagId = table.Column<int>(nullable: false),
                    Firma = table.Column<string>(nullable: true),
                    Hovedforløb = table.Column<int>(nullable: false),
                    ProfilTekst = table.Column<string>(nullable: true),
                    Speciale = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaerlingProfil", x => x.LaerlingID);
                    table.ForeignKey(
                        name: "FK_LaerlingProfil_Fag_FagId",
                        column: x => x.FagId,
                        principalTable: "Fag",
                        principalColumn: "FagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LaerlingProfilID",
                table: "AspNetUsers",
                column: "LaerlingProfilID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaerlingProfil_FagId",
                table: "LaerlingProfil",
                column: "FagId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_LaerlingProfil_LaerlingProfilID",
                table: "AspNetUsers",
                column: "LaerlingProfilID",
                principalTable: "LaerlingProfil",
                principalColumn: "LaerlingID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_LaerlingProfil_LaerlingProfilID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LaerlingProfil");

            migrationBuilder.DropTable(
                name: "Fag");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LaerlingProfilID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LaerlingProfilID",
                table: "AspNetUsers");
        }
    }
}
