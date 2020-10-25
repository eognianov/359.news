using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NewsSystem.Data.Migrations
{
    public partial class workerTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    TypeName = table.Column<string>(maxLength: 100, nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    RunAfter = table.Column<DateTime>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Processing = table.Column<bool>(nullable: false),
                    Processed = table.Column<bool>(nullable: false),
                    ProcessingComment = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTasks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerTasks");
        }
    }
}
