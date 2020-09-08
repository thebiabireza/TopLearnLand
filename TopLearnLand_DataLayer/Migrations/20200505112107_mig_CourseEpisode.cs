using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearnLand_DataLayer.Migrations
{
    public partial class mig_CourseEpisode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CourseEpisodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseEpisodes_CourseId",
                table: "CourseEpisodes",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEpisodes_Courses_CourseId",
                table: "CourseEpisodes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEpisodes_Courses_CourseId",
                table: "CourseEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_CourseEpisodes_CourseId",
                table: "CourseEpisodes");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseEpisodes");
        }
    }
}
