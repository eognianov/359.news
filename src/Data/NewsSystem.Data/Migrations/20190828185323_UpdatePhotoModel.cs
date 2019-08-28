using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsSystem.Data.Migrations
{
    public partial class UpdatePhotoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_UploaderId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_IsDeleted",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_UploaderId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "SecureUrl",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UploaderId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "Photos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "Photos",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Bytes",
                table: "Photos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Photos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Photos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecureUrl",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploaderId",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Photos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Photos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IsDeleted",
                table: "Photos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UploaderId",
                table: "Photos",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_UploaderId",
                table: "Photos",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
