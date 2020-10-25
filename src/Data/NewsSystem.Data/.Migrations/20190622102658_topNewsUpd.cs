using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsSystem.Data.Migrations
{
    public partial class topNewsUpd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_TopNews_TopNewsId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_TopNewsId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TopNewsId",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "TopNews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TopNews_NewsId",
                table: "TopNews",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopNews_News_NewsId",
                table: "TopNews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopNews_News_NewsId",
                table: "TopNews");

            migrationBuilder.DropIndex(
                name: "IX_TopNews_NewsId",
                table: "TopNews");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "TopNews");

            migrationBuilder.AddColumn<int>(
                name: "TopNewsId",
                table: "News",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_TopNewsId",
                table: "News",
                column: "TopNewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_TopNews_TopNewsId",
                table: "News",
                column: "TopNewsId",
                principalTable: "TopNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
