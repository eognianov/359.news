using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsSystem.Data.Migrations
{
    public partial class newsEntityPublishUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Published",
                table: "News",
                newName: "isPublished");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOn",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedOn",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "isPublished",
                table: "News",
                newName: "Published");
        }
    }
}
