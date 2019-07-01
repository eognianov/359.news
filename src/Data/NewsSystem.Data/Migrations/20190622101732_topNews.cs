using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsSystem.Data.Migrations
{
    public partial class topNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "TopNewsId",
                table: "News",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TopNews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopNews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_TopNewsId",
                table: "News",
                column: "TopNewsId");

            migrationBuilder.CreateIndex(
                name: "IX_TopNews_IsDeleted",
                table: "TopNews",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_News_TopNews_TopNewsId",
                table: "News",
                column: "TopNewsId",
                principalTable: "TopNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_TopNews_TopNewsId",
                table: "News");

            migrationBuilder.DropTable(
                name: "TopNews");

            migrationBuilder.DropIndex(
                name: "IX_News_TopNewsId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TopNewsId",
                table: "News");

           

            migrationBuilder.CreateIndex(
                name: "IX_Payments_NewsId",
                table: "Payments",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");
        }
    }
}
