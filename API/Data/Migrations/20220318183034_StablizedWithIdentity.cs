using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class StablizedWithIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhoto_AspNetUsers_AppUserId",
                table: "UserPhoto");

            migrationBuilder.DropIndex(
                name: "IX_UserPhoto_AppUserId",
                table: "UserPhoto");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserPhoto");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserPhoto",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "PhotoUserId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Addresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhotoUserId",
                table: "AspNetUsers",
                column: "PhotoUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProductId",
                table: "Comment",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserPhoto_PhotoUserId",
                table: "AspNetUsers",
                column: "PhotoUserId",
                principalTable: "UserPhoto",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserPhoto_PhotoUserId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhotoUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserPhoto",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "UserPhoto",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserPhoto_AppUserId",
                table: "UserPhoto",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhoto_AspNetUsers_AppUserId",
                table: "UserPhoto",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
