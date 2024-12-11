using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dataModel.Migrations
{
    /// <inheritdoc />
    public partial class AddMoviesAndActorsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    Genre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    CharacterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    movieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Actors_Movies",
                        column: x => x.movieID,
                        principalTable: "Movies",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actors_movieID",
                table: "Actors",
                column: "movieID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Iso2 = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Iso3 = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryID = table.Column<int>(type: "int", nullable: false),
                    Lat = table.Column<decimal>(type: "decimal(8,6)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Countries",
                        column: x => x.countryID,
                        principalTable: "Countries",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_countryID",
                table: "Cities",
                column: "countryID");
        }
    }
}
