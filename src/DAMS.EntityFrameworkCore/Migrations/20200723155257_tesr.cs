using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace DAMS.Migrations
{
    public partial class tesr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Dates = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NotifyBefore = table.Column<TimeSpan>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OneTimeEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NotifyBefore = table.Column<TimeSpan>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    NotifyStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimeEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeriodEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    PeriodType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NotifyBefore = table.Column<TimeSpan>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodEvents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomEvents");

            migrationBuilder.DropTable(
                name: "OneTimeEvents");

            migrationBuilder.DropTable(
                name: "PeriodEvents");
        }
    }
}
