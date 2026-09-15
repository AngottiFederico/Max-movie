using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Max_movie.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosterUrl",
                table: "Peliculas",
                newName: "PosterUrlPortada");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosterUrlPortada",
                table: "Peliculas",
                newName: "PosterUrl");
        }
    }
}
