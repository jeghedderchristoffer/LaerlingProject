using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaerlingProject.Data.Migrations
{
    public partial class FixedMistakeAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaerlingProfil_AspNetUsers_ApplicationUserId",
                table: "LaerlingProfil");

            migrationBuilder.DropIndex(
                name: "IX_LaerlingProfil_ApplicationUserId",
                table: "LaerlingProfil");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "LaerlingProfil",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LaerlingProfilID",
                table: "AspNetUsers",
                column: "LaerlingProfilID",
                unique: true);

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

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LaerlingProfilID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "LaerlingProfil",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaerlingProfil_ApplicationUserId",
                table: "LaerlingProfil",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LaerlingProfil_AspNetUsers_ApplicationUserId",
                table: "LaerlingProfil",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
