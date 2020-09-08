using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearnLand_DataLayer.Migrations
{
    public partial class mig_episodeIsdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpisodeCount",
                table: "CourseEpisodes");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CourseEpisodes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CourseEpisodes");

            migrationBuilder.AddColumn<int>(
                name: "EpisodeCount",
                table: "CourseEpisodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
