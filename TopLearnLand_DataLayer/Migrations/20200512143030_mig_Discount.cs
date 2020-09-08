using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearnLand_DataLayer.Migrations
{
    public partial class mig_Discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisCounts",
                columns: table => new
                {
                    DisCountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisCountCode = table.Column<int>(maxLength: 200, nullable: false),
                    Percent = table.Column<int>(nullable: false),
                    UsableCount = table.Column<int>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: true),
                    ToDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisCounts", x => x.DisCountId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisCounts");
        }
    }
}
