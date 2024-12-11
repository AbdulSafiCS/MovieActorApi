using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dataModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActorMovieRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_Movies",
                table: "Actors");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_Movies",
                table: "Actors",
                column: "movieID",
                principalTable: "Movies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_Movies",
                table: "Actors");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_Movies",
                table: "Actors",
                column: "movieID",
                principalTable: "Movies",
                principalColumn: "ID");
        }
    }
}
